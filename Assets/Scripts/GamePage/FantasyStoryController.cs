using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

namespace OpenAI
{
    public class FantasyStoryController : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private Text textArea;  // 顯示劇情的地方
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button textBoxButton;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Button hideUIButton;
        [SerializeField] private GameObject gameUI;

        [SerializeField] private GameObject optionChoicing;
        [SerializeField] private GameObject fourOptions;
        [SerializeField] private GameObject selfChoicingPanel;
        [SerializeField] private GameObject moveOnTip;


        [SerializeField] private Button option1Button;
        [SerializeField] private Button option2Button;
        [SerializeField] private Button option3Button;
        [SerializeField] private Button option4Button;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private Button testButton;
        [SerializeField] private AudioSource BackgroundSound;

        [SerializeField] private Button SaveButton;
        [SerializeField] private Button LoadButton;
        [SerializeField] private GameObject LoadingPanel;

        [SerializeField] private Button settingButton;
        [SerializeField] private GameObject WrongApiPanel;

        public static string JsonFilePath;



        // private OpenAIApi openai = new OpenAIApi();
        private OpenAIApi openai;




        private List<ChatMessage> messages = new List<ChatMessage>();
        private List<ChatMessage> filteredMessages = new List<ChatMessage>();
        private string recap = "";
        private int chatCount = 0;

        private string prompt = "請直接開始劇情";
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

        public static string BackgroundImagePath;

        // Lai
        public SaveLoadLegacy SaveLoadLegacy;
        public static bool from_book2 = false;


        private void Start()
        {
            openai = new OpenAIApi(PlayerPrefs.GetString("APIKey", ""));
            testButton.onClick.AddListener(Test);

            textBoxButton.onClick.AddListener(MoveOn);
            sendButton.onClick.AddListener(sendButtonAct);
            hideUIButton.onClick.AddListener(UIHiding);
            backgroundButton.onClick.AddListener(BackGroundClick);

            option1Button.onClick.AddListener(() => SendReply(option1Button));
            option2Button.onClick.AddListener(() => SendReply(option2Button));
            option3Button.onClick.AddListener(() => SendReply(option3Button));
            option4Button.onClick.AddListener(option4ButtonAct);

            SaveButton.gameObject.SetActive(false);
            LoadButton.gameObject.SetActive(false);
            LoadingPanel.gameObject.SetActive(false);


            if (from_book2 == false)
            { // 讀檔的過來的話就不用sendreply
                SendReply(null);
            }
            if (from_book2 == true)
            { // 從book2過來的
                canMove = true;
                LoadImageFromJson();
                SendPreviousReply(textBoxButton.GetComponentInChildren<Text>().text);
            }

            lastChangeTime = Time.time;
            int randomSoundInt = UnityEngine.Random.Range(1, 14);
            AudioClip newSoundClip = Resources.Load<AudioClip>("GameMusic/Fantasy/" + randomSoundInt);
            BackgroundSound.clip = newSoundClip;
            BackgroundSound.enabled = true;
            BackgroundSound.Play();
        }

        private void Update()
        {

            if (from_book2 == true)
            {
                // if(textBoxCount >= 0 && textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] != null && suspend == false){
                //     MoveOn(); // 一直跳到有選項出現
                // }
                textBoxCount = 1000;
                textArea.text = currentFullTexts[currentFullTexts.Length - 1];
                if (getOptionDone)
                {
                    optionChoicing.SetActive(true);
                }
            }

            if (currentMessageRec && suspend == false)
            {
                currentFullTexts = currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text.Split("\n");
            }
            if (textBoxCount >= 0 && textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] != null && suspend == false)
            {
                //監測字串變化
                if (textArea.text != currentFullTexts[textBoxCount])
                {
                    textArea.text = currentFullTexts[textBoxCount];
                    lastChangeTime = Time.time;
                    canMove = false;
                }

                if (Time.time - lastChangeTime >= 1f && !canMove)
                {
                    if (textBoxCount == 0 && imgNeedChange)
                    {
                        ChangeImage(currentFullTexts[0]);
                        imgNeedChange = false;
                    }
                    canMove = true;
                }
            }

            if (canMove)
            {
                moveOnTip.SetActive(true);
            }
            else
            {
                moveOnTip.SetActive(false);
            }
        }

        private void Test()
        {

            foreach (ChatMessage m in filteredMessages)
            {
                print(m.Role + ":" + m.Content);
            }
            print(chatCount);
        }

        private async void SendReply(Button button)
        {
            from_book2 = false; // reset
            optionChoicing.SetActive(false);
            SaveButton.gameObject.SetActive(false);
            LoadButton.gameObject.SetActive(false);
            try
            {
                textBoxCount = 0;
                imgNeedChange = true;
                getOptionDone = false;

                string userContent = "";
                if (button)
                {
                    userContent = button.GetComponentInChildren<Text>().text;
                }
                else
                {
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

                if (messages.Count == 0)
                {
                    sentMessage.Content = prompt + "\n" + inputField.text;
                }
                else
                {
                    var sentItem = AppendMessage(sentMessage);
                    currentMessageRec = sentItem;
                    textArea.text = currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text; // 這裡!!!
                    suspend = true;
                    textBoxCount = -1;
                }
                var recItem = AppendMessage(recMessage);

                messages.Add(sentMessage);
                filteredMessages.Add(sentMessage);//此send前的濃縮若慢到這之後才結束會導致刪除到這段記憶，但是此內容基本上會直接影響到或被下段assistant內容包含故影響不大

                inputField.text = "";
                inputField.enabled = false;
                sendButton.enabled = false;

                currentMessageRec = recItem;
                // Complete the prompt
                List<ChatMessage> sendMessages = new List<ChatMessage>(filteredMessages);
                var systemMessage = new ChatMessage()
                {
                    Role = "system",
                    Content = "請和我玩奇幻劇情遊戲，遊戲過程不停根據我的輸入給予我新的奇幻世界探索劇情，劇情請以第一人稱視角進行並且盡可能充滿細節和豐富互動性，劇情節奏請慢慢來使我有更多時機能針對劇情做出選擇，遇到任何可供選擇的劇情點就停下詢問我我想怎麼做，每次給予的劇情不要一次太多，盡量小於300字"
                    // Content = "給我一個四字成語，不要回答超過四個字"

                };
                sendMessages.Add(systemMessage);
                foreach (ChatMessage m in sendMessages)
                {
                    print("[SEND]" + m.Role + ":" + m.Content);
                }
                semaphore = new SemaphoreSlim(0);
                openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
                {
                    Model = "gpt-3.5-turbo-0613",
                    Messages = sendMessages,
                    Temperature = 1f,
                    //MaxTokens = 1024,
                    Stream = true
                }, (responses) => HandleResponse(responses, recMessage, recItem), HandleComplete, token);
                await semaphore.WaitAsync();

                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                recItem.anchoredPosition = new Vector2(0, -height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(recItem);
                height += recItem.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;

                recMessage.Content = recItem.GetChild(0).GetChild(0).GetComponent<Text>().text;
                messages.Add(recMessage);
                filteredMessages.Add(recMessage);//此send前的濃縮若慢到這之後才結束會導致刪除到這段記憶，影響嚴重，但基本上不可能那麼慢


                chatCount++;
                if (chatCount >= 2)
                {
                    messageFilter();
                    chatCount = 0;
                }

                // 存message
                SaveLoadLegacy.SaveChatMassage(messages);
                SaveLoadLegacy.SaveStoryToList(recMessage.Content);


                GetOptions(recMessage.Content);

                inputField.enabled = true;
                sendButton.enabled = true;

            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }

        private async void SendPreviousReply(string story)
        {
            optionChoicing.SetActive(false);
            try
            {
                textBoxCount = 0;


                if (from_book2 == true)
                {
                    imgNeedChange = false;
                }
                else
                {
                    imgNeedChange = true;
                }
                getOptionDone = false;

                var recMessage = new ChatMessage()
                {
                    Role = "assistant",
                    Content = story
                };

                var recItem = AppendMessage(recMessage);


                inputField.text = "";
                inputField.enabled = false;
                sendButton.enabled = false;

                currentMessageRec = recItem;


                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                recItem.anchoredPosition = new Vector2(0, -height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(recItem);
                height += recItem.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;

                recMessage.Content = recItem.GetChild(0).GetChild(0).GetComponent<Text>().text;
                messages.Add(recMessage);
                filteredMessages.Add(recMessage);//此send前的濃縮若慢到這之後才結束會導致刪除到這段記憶，影響嚴重，但基本上不可能那麼慢


                chatCount++;
                if (chatCount >= 2)
                {
                    messageFilter();
                    chatCount = 0;
                }

                if (from_book2 == false)
                {
                    GetOptions(recMessage.Content);
                }
                else
                {
                    GetOptionsFromJson();
                }

                inputField.enabled = true;
                sendButton.enabled = true;

            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }

        private async void messageFilter()
        {
            List<ChatMessage> toFilMessages = new List<ChatMessage>(filteredMessages);
            var toS = new ChatMessage()
            {
                Role = "user",
                Content = "請擷取以上對話重要內容以利後續記憶"
            };
            toFilMessages.Add(toS);

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = toFilMessages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                recap = completionResponse.Choices[0].Message.Content;
                print("[RECAP]:\n" + recap);
                filteredMessages.Clear();
                var s = new ChatMessage()
                {
                    Role = "assistant",
                    Content = "前情提要: " + recap
                };
                filteredMessages.Add(s);
            }
            else
            {
                Debug.LogWarning("Can't filter!");
            }
        }

        private void MoveOn()
        {
            if (canMove)
            {
                suspend = false;
                textBoxCount++;
                if (textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] == "")
                {
                    textBoxCount++;
                }
                if (textBoxCount >= currentFullTexts.Length && getOptionDone)
                {
                    if (from_book2 = true)
                    {
                        LoadingPanel.gameObject.SetActive(false);
                    }
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

        private void HandleResponse(List<CreateChatCompletionResponse> responses, ChatMessage message, RectTransform item)
        {
            try
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
            catch (Exception ex)
            {
                WrongApiPanel.SetActive(true);
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }

        private void HandleComplete()
        {
            semaphore.Release();
        }

        private async void GetOptions(string fullPlot)
        {
            try
            {
                var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
                {
                    Prompt = "請根據以下劇情給予我三個選項\n\n劇情:\n" + fullPlot + "\n\n請以換行符分隔三個選項:\n",
                    Model = "text-davinci-003",
                    MaxTokens = 256,
                    Temperature = 0.0f,
                });

                print("[OPTIONS]:\n" + completionResponse.Choices[0].Text.Trim());
                string[] optionList = completionResponse.Choices[0].Text.Trim().Split('\n');
                //字串處理
                string[] filteredOptions = optionList.Where(option => !string.IsNullOrEmpty(option)).ToArray();
                for (int i = 0; i < filteredOptions.Length; i++)
                {
                    //filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\da-zA-Z.()]+", "");
                    filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\da-zA-Z.()\n]+", "");
                }

                option1Button.GetComponentInChildren<Text>().text = filteredOptions[0];
                option2Button.GetComponentInChildren<Text>().text = filteredOptions[1];
                option3Button.GetComponentInChildren<Text>().text = filteredOptions[2];
                // 顯示存檔和讀檔的按鈕
                SaveButton.gameObject.SetActive(true);
                LoadButton.gameObject.SetActive(true);

                getOptionDone = true;
            }
            catch (Exception ex)
            {
                WrongApiPanel.SetActive(true);
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }
        private async void GetOptionsFromJson()
        {
            GameData data = OpenAI.SaveLoadLegacy.GetGameData(JsonFilePath);
            option1Button.GetComponentInChildren<Text>().text = data.LatestOption1;
            option2Button.GetComponentInChildren<Text>().text = data.LatestOption2;
            option3Button.GetComponentInChildren<Text>().text = data.LatestOption3;

            getOptionDone = true;
        }

        private async void ChangeImage(string plot)
        {
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請判斷以下劇情應該發生在哪一個場景?\n\n劇情:\n" + plot + "\n\n(1)城堡\n(2)城市\n(3)塔樓\n(4)山林\n(5)巢穴\n(6)星塵\n(7)星雲\n(8)森林\n(9)沙漠\n(10)洞穴\n(11)湖泊\n(12)瀑布\n(13)秘境\n(14)花園\n(15)銀河\n(16)魔法學院\n\n請直接回答數字就好:\n",
                Model = "text-davinci-003",
                MaxTokens = 128,
                Temperature = 0.0f,
                Stop = "."
            });

            //確保只有一個數字
            string cleanedString = "";
            foreach (char c in completionResponse.Choices[0].Text.Trim())
            {
                if (char.IsDigit(c))
                {
                    cleanedString += c;
                }
            }
            //若類別碼無成功給予防範機制(給予隨機類別)
            if (cleanedString.Length > 2)
            {
                print("![圖片類別取得失敗]");
                cleanedString = UnityEngine.Random.Range(1, 31).ToString();
            }

            //換圖
            int randomInt = UnityEngine.Random.Range(1, 5);
            print("[圖片類別編號]: " + cleanedString + "\n[圖片隨機碼]: " + randomInt);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + cleanedString + "/" + randomInt);
            backgroundImage.sprite = newSprite;
            // 提供給SaveLoad script存取
            string backgroundImagePath = "FantasyBackground/" + cleanedString + "/" + randomInt;
            FantasyStoryController.BackgroundImagePath = backgroundImagePath;


        }
        private async void LoadImageFromJson()
        {

            string backgroundImagePath = OpenAI.SaveLoadLegacy.GetGameData(JsonFilePath).BackgroundImg; // 圖片路徑
            Sprite newSprite = Resources.Load<Sprite>(backgroundImagePath);
            backgroundImage.sprite = newSprite;
            Debug.Log("backgroundImagePath: " + backgroundImagePath);

        }

        private void option4ButtonAct()
        {
            fourOptions.SetActive(false);
            selfChoicingPanel.SetActive(true);
            SaveButton.gameObject.SetActive(false);
            LoadButton.gameObject.SetActive(false);
        }

        private void sendButtonAct()
        {
            fourOptions.SetActive(true);
            selfChoicingPanel.SetActive(false);
            SendReply(null);
        }
        private void UIHiding()
        {
            gameUI.SetActive(false);
            UIHideing = true;
        }

        private void BackGroundClick()
        {
            if (UIHideing == true)
            {
                gameUI.SetActive(true);
                UIHideing = false;
            }
            else
            {
                MoveOn();
            }
        }


    }
}