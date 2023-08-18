using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;



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

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private Button testButton;

        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();

        //private string prompt = "和我玩武俠劇情遊戲";
        private string prompt = "你好";

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

        private void Start()
        {
            testButton.onClick.AddListener(Test);

            textBoxButton.onClick.AddListener(MoveOn);
            sendButton.onClick.AddListener(SendReply);
            hideUIButton.onClick.AddListener(UIHiding);
            backgroundButton.onClick.AddListener(BackGroundClick);
            SendReply();

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
                
                if(Time.time - lastChangeTime >= 0.5f && !canMove){
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
            //print(textBoxCount);
            //string p = "在青鳥村度過了許多平靜的日子後，有一天，一位神秘的訪客來到了村子，他自稱是「黑影刺客」，聲稱要挑戰村中最強的劍客。村子裡的人們都感到驚恐，不知如何是好。";
            //ChangeImage(p);
            //隨機換背景
            // int randomInt = UnityEngine.Random.Range(1,5);
            // Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + randomInt);
            // backgroundImage.sprite = newSprite;
        }

        private async void SendReply()
        {
            try{
                textBoxCount = 0;
                imgNeedChange = true;
                var sentMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = inputField.text
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

        private async void ChangeImage(string plot){
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請判斷以下劇情應該發生在哪一個場景?\n\n劇情:\n" + plot +"\n\n(1)小村莊\n(2)山谷\n(3)寺廟\n\n請直接回答數字就好:\n",
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
            print("圖片編號: " + cleanedString);

            //換圖
            Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + cleanedString);
            backgroundImage.sprite = newSprite;
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

