using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


namespace OpenAI
{
    public class JsonController : MonoBehaviour
    {
        [SerializeField] private Text textArea;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button textBoxButton;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Button hideUIButton;
        [SerializeField] private GameObject gameUI;

        [SerializeField] private GameObject optionChoicing;
        [SerializeField] private GameObject fourOptions;

        [SerializeField] private Button option1Button;
        [SerializeField] private Button option2Button;
        [SerializeField] private Button option3Button;
        [SerializeField] private Button option4Button;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private Button testButton;

        private OpenAIApi openai = new OpenAIApi();


        private CancellationTokenSource token = new CancellationTokenSource();
        private SemaphoreSlim semaphore;

        private float height = 0;
        private RectTransform currentMessageRec;
        private bool suspend = false;
        private string[] currentFullTexts = new string[50];
        private int textBoxCount = 0;
        private int textLength = 0;
        private bool showAll = false;
        private int filteredCount = 0;
        private string[] filteredMessage;
        private bool UIHideing = false;
        private bool canMove = false;
        private float lastChangeTime;
        private bool imgNeedChange = false;
        private bool getOptionDone = false;
        StoryData data;
        Theme theme;
        Story story;
        Block block;

        private void Start()
        {



            textBoxButton.onClick.AddListener(MoveOn);
            hideUIButton.onClick.AddListener(UIHiding);
            backgroundButton.onClick.AddListener(BackGroundClick);

            option1Button.onClick.AddListener(() => SendReply(option1Button));
            option2Button.onClick.AddListener(() => SendReply(option2Button));
            option3Button.onClick.AddListener(() => SendReply(option3Button));
            option4Button.onClick.AddListener(option4ButtonAct);

            createGame();
            SendReply(null);

            lastChangeTime = Time.time;
        }

        private void Update()
        {
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
        }



        private async void SendReply(Button button)
        {
            StoryData data = LoadStoryData();
            optionChoicing.SetActive(false);
            string option = "";
            if (button)
            {
                option = button.GetComponentInChildren<Text>().text;
            }
            else
            {
                option = "Init";
            }
            try
            {
                textBoxCount = 0;
                imgNeedChange = true;
                getOptionDone = false;



                block = story.blocks[option];
                Debug.Log(block.plot);


                string sentMessage = block.plot;
                string recMessage = "";
                textLength = sentMessage.Length;
                // var sentItem = AppendMessage(sentMessage);
                // currentMessageRec = sentItem;

                string[] messageList = sentMessage.Split('。');
                //字串處理
                filteredMessage = messageList.Where(option => !string.IsNullOrEmpty(option)).ToArray();
                // for (int i = 0; i < filteredMessage.Length; i++)
                // {
                //     filteredMessage[i] = Regex.Replace(filteredMessage[i], @"[\d.()\n]+", "");
                // }

                textArea.text = "";
                filteredCount = 0;
                textBoxCount = 0;

                StartCoroutine(TypeSentence(sentMessage, textArea));

                // textArea.text = currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text;
                suspend = true;

                // var recItem = AppendMessage(recMessage);


                // currentMessageRec = recItem;

                // Complete the prompt
                // semaphore = new SemaphoreSlim(0);
                // openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
                // {
                //     Model = "gpt-3.5-turbo-0613",
                //     Messages = messages,
                //     Stream = true
                // }, (responses) => HandleResponse(responses, recMessage, recItem), HandleComplete, token);
                // await semaphore.WaitAsync();

                // scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                // recItem.anchoredPosition = new Vector2(0, -height);
                // LayoutRebuilder.ForceRebuildLayoutImmediate(recItem);
                // height += recItem.sizeDelta.y;
                // scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                // scroll.verticalNormalizedPosition = 0;

                // recMessage = recItem.GetChild(0).GetChild(0).GetComponent<Text>().text;

                GetOptions(block);


            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }
        bool canContinueTyping = true;

        IEnumerator TypeSentence(string sentence, Text targetTextComponent)
        {
            targetTextComponent.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                textBoxCount++;
                // if (showAll == true)
                // {
                //     canContinueTyping = false;
                //     while (!canContinueTyping) // 等待直到 MoveOn 被调用
                //     {
                //         yield return null;
                //     }
                //     filteredCount++;
                // }
                if (showAll && letter != '。')
                {
                    targetTextComponent.text = filteredMessage[filteredCount];
                }
                else if (letter == '。')
                {
                    textBoxCount++;
                    canContinueTyping = false;
                    filteredCount++;
                    showAll = false;
                    while (!canContinueTyping) // 等待直到 MoveOn 被调用
                    {
                        yield return null;
                    }
                }
                else
                {
                    targetTextComponent.text += letter;
                    yield return new WaitForSeconds(0.05f);

                }



            }
            suspend = true;
        }

        void MoveOn()
        {
            // showAll = true;
            // if (canContinueTyping == true)
            // {
            //     textArea.text = filteredMessage[filteredCount];
            //     canContinueTyping = false;
            // }
            if (canContinueTyping == true)
            {
                showAll = true;
            }
            canContinueTyping = true; // 允许 TypeSentence 继续输出文字

            if (textBoxCount >= textLength)
            {
                optionChoicing.SetActive(true);
            }
            else
            {
                textArea.text = "";
                Debug.Log(textBoxCount);
                Debug.Log(textLength);
            }
        }


        // private RectTransform AppendMessage(string message)
        // {
        //     scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        //     var item = Instantiate(sent, scroll.content);
        //     // StartCoroutine(TypeSentence(message, textArea));
        //     item.anchoredPosition = new Vector2(0, -height);
        //     LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        //     height += item.sizeDelta.y;
        //     scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        //     scroll.verticalNormalizedPosition = 0;

        //     return item;
        // }



        private async void GetOptions(Block currentBlock)
        {

            var OptionKey = currentBlock.options.Keys.GetEnumerator();
            OptionKey.MoveNext();
            string Key = OptionKey.Current;
            string optionValue = currentBlock.options[Key];

            string[] optionList = optionValue.Split('\n');
            //字串處理
            string[] filteredOptions = optionList.Where(option => !string.IsNullOrEmpty(option)).ToArray();
            // for (int i = 0; i < filteredOptions.Length; i++)
            // {
            //     filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\d.()\n]+", "");
            // }

            option1Button.GetComponentInChildren<Text>().text = filteredOptions[0];
            option2Button.GetComponentInChildren<Text>().text = filteredOptions[1];
            option3Button.GetComponentInChildren<Text>().text = filteredOptions[2];

            getOptionDone = true;
        }
        private async void ChangeImage(string plot)
        {
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請判斷以下劇情應該發生在哪一個場景?\n\n劇情:\n" + plot + "\n\n(1)小村莊\n(2)山谷\n(3)山洞\n(4)寺廟\n(5)遺跡廢墟\n(6)竹林\n(7)瀑布\n(8)荒野\n(9)市集\n(10)擂台\n(11)酒樓\n(12)客棧\n(13)武學門派\n(14)武林聚會\n(15)城牆\n(16)山寨\n(17)密室\n(18)山谷涼亭\n(19)山間小徑\n(20)軍營\n(21)冰川\n(22)沙漠\n(23)皇宮\n(24)雪山\n(25)森林\n(26)港口\n(27)湖泊\n(28)衙門\n(29)戰場\n(30)懸崖\n\n請直接回答數字就好:\n",
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
            print("圖片類別編號: " + cleanedString);

            //換圖
            int randomInt = UnityEngine.Random.Range(1, 5);
            print("圖片隨機碼: " + randomInt);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + cleanedString + "/" + randomInt);
            backgroundImage.sprite = newSprite;
        }

        private void option4ButtonAct()
        {
            fourOptions.SetActive(false);
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
        private void createGame()
        {
            data = LoadStoryData();
            theme = data.themes[0];
            story = theme.stories[0];
        }
        StoryData LoadStoryData()
        {
            TextAsset jsonData = Resources.Load<TextAsset>("Story/data");
            return JsonConvert.DeserializeObject<StoryData>(jsonData.text);
        }
    }

}


