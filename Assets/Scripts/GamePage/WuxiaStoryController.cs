using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OpenAI
{
    public class WuxiaStoryController : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private Text textArea;
        [SerializeField] private Button backgroundButton;  
        [SerializeField] private Button textBoxButton;  
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Button hideUIButton;
        [SerializeField] private GameObject gameUI;

        [SerializeField] private GameObject optionChoicing;
        [SerializeField] private GameObject fourOptions;
        [SerializeField] private GameObject selfChoicingPanel;

        [SerializeField] private Button option1Button;
        [SerializeField] private Button option2Button;
        [SerializeField] private Button option3Button;
        [SerializeField] private Button option4Button;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private Button testButton;

        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();

        private string prompt = "和我玩武俠劇情遊戲";
        //private string prompt = "你好";

        private CancellationTokenSource token = new CancellationTokenSource();
        private SemaphoreSlim semaphore;

        private float height = 0;
        private RectTransform currentMessageRec;
        private bool suspend = false;
        private string[] currentFullTexts = new string[50];
        private int textBoxCount = 0;
        private bool UIHideing = false;
        private bool canMove = false;
        private float lastChangeTime;
        private bool imgNeedChange = false;
        private bool getOptionDone = false;

        private void Start()
        {
            testButton.onClick.AddListener(Test);

            textBoxButton.onClick.AddListener(MoveOn);
            sendButton.onClick.AddListener(sendButtonAct);
            hideUIButton.onClick.AddListener(UIHiding);
            backgroundButton.onClick.AddListener(BackGroundClick);

            option1Button.onClick.AddListener(() => SendReply(option1Button));
            option2Button.onClick.AddListener(() => SendReply(option2Button));
            option3Button.onClick.AddListener(() => SendReply(option3Button));
            option4Button.onClick.AddListener(option4ButtonAct);

            SendReply(null);

            lastChangeTime = Time.time;
        }

        private void Update(){
            if(currentMessageRec && suspend == false){
                currentFullTexts = currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text.Split("\n");
            }
            if(textBoxCount >= 0 && textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] != null && suspend == false){
                //監測字串變化
                if(textArea.text != currentFullTexts[textBoxCount]){
                    textArea.text = currentFullTexts[textBoxCount];
                    lastChangeTime = Time.time;
                    canMove = false;
                }
                
                if(Time.time - lastChangeTime >= 1f && !canMove){
                    if(textBoxCount == 0 && imgNeedChange){
                        ChangeImage(currentFullTexts[0]);
                        imgNeedChange = false;
                    }
                    canMove = true;
                }
            }
        }

        private void Test(){
            //print(currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text);
            // foreach (ChatMessage message in messages){
            //     print(message.Role + ":" +message.Content);
            // }
            //print(canMove);
            print(textBoxCount);
            //string p = "在青鳥村度過了許多平靜的日子後，有一天，一位神秘的訪客來到了村子，他自稱是「黑影刺客」，聲稱要挑戰村中最強的劍客。村子裡的人們都感到驚恐，不知如何是好。";
            //ChangeImage(p);
            //隨機換背景
            // int randomInt = UnityEngine.Random.Range(1,5);
            // Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + randomInt);
            // backgroundImage.sprite = newSprite;
            // string full = "\nsdfa532.古代中國66dc.\n\n古代657.0日本\n\n現91.c代社會PJI";
            // string[] optionList = full.Split('\n');
            // string[] filteredOptions = optionList.Where(option => !string.IsNullOrEmpty(option)).ToArray();
            // for (int i = 0; i < filteredOptions.Length; i++)
            // {
            //     //filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\da-zA-Z.()]+", "");
            //     filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\d.()]+", "");
            //     print(filteredOptions[i]);
            // }
        }

        private async void SendReply(Button button)
        {
            optionChoicing.SetActive(false);
            try{
                textBoxCount = 0;
                imgNeedChange = true;
                getOptionDone = false;

                string userContent = "";
                if(button){
                    userContent = button.GetComponentInChildren<Text>().text;
                }else{
                    userContent = inputField.text;
                }
                var sentMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = userContent
                };
                var recMessage = new ChatMessage()
                {
                    Role = "assistant",
                    Content = ""
                };

                if (messages.Count == 0){
                    sentMessage.Content = prompt + "\n" + inputField.text; 
                }else{
                    var sentItem = AppendMessage(sentMessage);
                    currentMessageRec = sentItem;
                    textArea.text = currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text;
                    suspend = true;
                    textBoxCount = -1;
                }
                var recItem = AppendMessage(recMessage);

                messages.Add(sentMessage);

                inputField.text = "";
                inputField.enabled = false;
                sendButton.enabled = false;

                currentMessageRec = recItem;
                // Complete the prompt
                semaphore = new SemaphoreSlim(0);
                openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
                {
                    Model = "gpt-3.5-turbo-0613",
                    Messages = messages,
                    Stream = true
                },(responses) => HandleResponse(responses, recMessage, recItem),HandleComplete,token);
                await semaphore.WaitAsync();

                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                recItem.anchoredPosition = new Vector2(0, -height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(recItem);
                height += recItem.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;

                recMessage.Content = recItem.GetChild(0).GetChild(0).GetComponent<Text>().text;
                messages.Add(recMessage);

                GetOptions(recMessage.Content);

                inputField.enabled = true;
                sendButton.enabled = true;
            }catch(Exception ex){
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }

        private void MoveOn(){
            if(canMove){
                suspend = false;
                textBoxCount++;
                if(textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] == ""){
                    textBoxCount++;
                }
                if(textBoxCount >= currentFullTexts.Length && getOptionDone){
                    optionChoicing.SetActive(true);
                }
            }
        }
        private RectTransform AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;

            return item;
        }

        private void HandleResponse(List<CreateChatCompletionResponse> responses, ChatMessage message,RectTransform item)
        {
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

                message.Content = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
                item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;

                item.anchoredPosition = new Vector2(0, -height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(item);
                // height += item.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;
        }

        private void HandleComplete(){
            semaphore.Release();
        }

        private async void GetOptions(string fullPlot)
        {
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請根據以下劇情給予我三個選項\n\n劇情:\n" + fullPlot + "\n\n請以換行符分隔三個選項:\n",
                Model = "text-davinci-003",
                MaxTokens = 256,
                Temperature = 0.0f,
            });

            print("ALL: "+completionResponse.Choices[0].Text.Trim());
            string[] optionList = completionResponse.Choices[0].Text.Trim().Split('\n');
            //字串處理
            string[] filteredOptions = optionList.Where(option => !string.IsNullOrEmpty(option)).ToArray();
            for (int i = 0; i < filteredOptions.Length; i++)
            {
                //filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\da-zA-Z.()]+", "");
                filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\d.()\n]+", "");
            }

            option1Button.GetComponentInChildren<Text>().text = filteredOptions[0];
            option2Button.GetComponentInChildren<Text>().text = filteredOptions[1];
            option3Button.GetComponentInChildren<Text>().text = filteredOptions[2];

            getOptionDone = true;
        }
        private async void ChangeImage(string plot){
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請判斷以下劇情應該發生在哪一個場景?\n\n劇情:\n" + plot +"\n\n(1)小村莊\n(2)山谷\n(3)山洞\n(4)寺廟\n(5)遺跡廢墟\n(6)竹林\n(7)瀑布\n(8)荒野\n(9)市集\n(10)擂台\n(11)酒樓\n(12)客棧\n(13)武學門派\n(14)武林聚會\n(15)城牆\n(16)山寨\n(17)密室\n(18)山谷涼亭\n(19)山間小徑\n(20)軍營\n(21)冰川\n(22)沙漠\n(23)皇宮\n(24)雪山\n(25)森林\n(26)港口\n(27)湖泊\n(28)衙門\n(29)戰場\n(30)懸崖\n\n請直接回答數字就好:\n",
                Model = "text-davinci-003",
                MaxTokens = 128,
                Temperature = 0.0f,
                Stop="."
            });

            //確保只有一個數字
            string cleanedString = "";
            foreach (char c in completionResponse.Choices[0].Text.Trim()){
                if(char.IsDigit(c)){
                    cleanedString += c;
                }
            }
            print("圖片類別編號: " + cleanedString);

            //換圖
            int randomInt = UnityEngine.Random.Range(1,5);
            print("圖片隨機碼: " + randomInt);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + cleanedString + "/" + randomInt);
            backgroundImage.sprite = newSprite;
        }

        private void option4ButtonAct(){
            fourOptions.SetActive(false);
            selfChoicingPanel.SetActive(true);
        }

        private void sendButtonAct(){
            fourOptions.SetActive(true);
            selfChoicingPanel.SetActive(false);
            SendReply(null);
        }
        private void UIHiding(){
            gameUI.SetActive(false);
            UIHideing = true;
        }

        private void BackGroundClick(){
            if(UIHideing == true){
                gameUI.SetActive(true);
                UIHideing = false;
            }else{
                MoveOn();
            }
        }
    }
}

