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
    public class NewStoryController : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private Image image;
        [SerializeField] private GameObject WrongApiPanel;
        [SerializeField] private GameObject optionChoicing;
        [SerializeField] private GameObject selfChoicing;

        [SerializeField] private Button option1;
        [SerializeField] private Button option2;
        [SerializeField] private Button option3;
        [SerializeField] private Button option4;

        private float height;
        private OpenAIApi openai = new OpenAIApi(InputFieldManager.user_api);

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "我現在要跟你玩文字遊戲。故事背景設定在台灣的白色恐怖時期，請確認好資訊無誤再放入故事中，遊玩視角為第二人稱。請詳細敘述主角目前的所在地、場景、正在發生什麼事情、會聽到、看到什麼東西或建築物，當我問出有關當時造就的情況的問題時，請以正確的資訊教導我。首先請生成150字的故事開頭，第一句話以:你是 {主角名字}，{身分} ,開頭，之後以第二人稱視角敘述周遭環境，必要時也可以以旁白角度描寫事件發生經過、場景描述等。之後我會根據劇情輸入主角（我）後續的動作，再依照我的輸入產生出下一個篇幅為50~100字的劇情，繼續引導故事伏筆前進，貼近當時的歷史背景，適時給我一些線索去探索，盡量在回覆的結尾拋給我一個問題，最後預設一個結尾，引導我到結尾即遊戲結束";

        private string currentFullText = "";
        private string imgGenerateText = "";
        private string userInput = "";

        private void Start()
        {
            optionChoicing.SetActive(false);

            // CreateNewGame Variable
            // string gameMode,gameStyle,gamePicQuality,gameDirection,gameLanguage;
            // if(CreateNewGameButton.gamedir==null)
            // {
            //     gameMode = CreateNewGameButton.buttonTexts[0];
            //     gameStyle = CreateNewGameButton.buttonTexts[1];
            //     gamePicQuality = CreateNewGameButton.buttonTexts[2];
            //     gameDirection = CreateNewGameButton.buttonTexts[3];
            //     gameLanguage = CreateNewGameButton.buttonTexts[4];
            // }
            // else
            // {
            //     gameDirection = CreateNewGameButton.gamedir;
            //     gameMode = CreateNewGameButton.buttonTexts[0];
            //     gameStyle = CreateNewGameButton.buttonTexts[1];
            //     gamePicQuality = CreateNewGameButton.buttonTexts[2];
            //     gameLanguage = CreateNewGameButton.buttonTexts[3];
            // }
            // if(gameLanguage=="中文")
            // {
            //     prompt = "我現在要跟你玩文字遊戲。故事背景設定在台灣的"+gameMode+"，請確認好資訊無誤再放入故事中，遊玩視角為第二人稱。請詳細敘述主角目前的所在地、場景、正在發生什麼事情、會聽到、看到什麼東西、對建築物的描述也請符合"+gameMode+"，當我問出有關當時造就的情況的問題時，請以正確的資訊教導我。首先請生成150字的故事開頭，第一句話以:你是 {主角名字}，{身分} ,開頭，之後以第二人稱視角敘述周遭環境，必要時也可以以旁白角度描寫事件發生經過、場景描述等。之後我會根據劇情輸入主角（我）後續的動作，再依照我的輸入產生出下一個篇幅為50~100字的劇情，繼續引導故事伏筆前進，貼近當時的歷史背景，適時給我一些線索去探索，盡量在回覆的結尾拋給我一個問題，最後預設一個結尾，引導我到結尾即遊戲結束";
            // }
            // else
            // {
            //     prompt = "我現在要跟你玩文字遊戲。故事背景設定在台灣的"+gameMode+"，請確認好資訊無誤再放入故事中，遊玩視角為第二人稱。請詳細敘述主角目前的所在地、場景、正在發生什麼事情、會聽到、看到什麼東西、對建築物的描述也請符合"+gameMode+"，當我問出有關當時造就的情況的問題時，請以正確的資訊教導我。首先請生成150字的故事開頭，第一句話以:你是 {主角名字}，{身分} ,開頭，之後以第二人稱視角敘述周遭環境，必要時也可以以旁白角度描寫事件發生經過、場景描述等。之後我會根據劇情輸入主角（我）後續的動作，再依照我的輸入產生出下一個篇幅為50~100字的劇情，繼續引導故事伏筆前進，貼近當時的歷史背景，適時給我一些線索去探索，盡量在回覆的結尾拋給我一個問題，最後預設一個結尾，引導我到結尾即遊戲結束";
            //     //prompt = "You are now acting as a game terminal, generate plot development according to my prompts. \nQ: ";
            // }

            SendReply();
            button.onClick.AddListener(SendReply);
            option1.onClick.AddListener(() => SendReplyButton(option1));
            option2.onClick.AddListener(() => SendReplyButton(option2));
            option3.onClick.AddListener(() => SendReplyButton(option3));
            option4.onClick.AddListener(option4Act);

            // Debug.Log(gameMode+gameStyle+gamePicQuality+gameDirection+gameLanguage);
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply(){
            selfChoicing.SetActive(false);
            option1.interactable = true;
            option2.interactable = true;
            option3.interactable = true;
            option4.interactable = true;
            try{
                var newMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = inputField.text
                };
                
                AppendMessage(newMessage);

                if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text; 
                
                messages.Add(newMessage);
                
                userInput = inputField.text;
                //button.enabled = false;
                inputField.text = "";
                //inputField.enabled = false;
                optionChoicing.SetActive(false);
                
                // Complete the prompt
                var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
                {
                    Model = "gpt-3.5-turbo-0613",
                    Messages = messages
                });

                if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
                {
                    var message = completionResponse.Choices[0].Message;
                    message.Content = message.Content.Trim();
                    
                    messages.Add(message);
                    AppendMessage(message);

                    GetOptions(option1);
                    GetOptions(option2);
                    GetOptions(option3);
                    //SendImageRequest();
                }
                else
                {
                    Debug.LogWarning("No text was generated from this prompt.");
                }

                //button.enabled = true;
                //inputField.enabled = true;
                optionChoicing.SetActive(true);
            }
            catch(Exception ex)
            {
                WrongApiPanel.SetActive(true);
            }
        }

        private async void SendReplyButton(Button button){
            try{
                var newMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = button.GetComponentInChildren<Text>().text
                };
                
                AppendMessage(newMessage);

                if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text; 
                
                messages.Add(newMessage);
                
                //button.enabled = false;
                inputField.text = "";
                //inputField.enabled = false;
                optionChoicing.SetActive(false);
                
                // Complete the prompt
                var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
                {
                    Model = "gpt-3.5-turbo-0613",
                    Messages = messages
                });

                if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
                {
                    var message = completionResponse.Choices[0].Message;
                    message.Content = message.Content.Trim();
                    
                    messages.Add(message);
                    AppendMessage(message);

                    GetOptions(option1);
                    GetOptions(option2);
                    GetOptions(option3);
                    //SendImageRequest();
                }
                else
                {
                    Debug.LogWarning("No text was generated from this prompt.");
                }

                //button.enabled = true;
                //inputField.enabled = true;
                optionChoicing.SetActive(true);
            }
            catch(Exception ex)
            {
                WrongApiPanel.SetActive(true);
            }            
        }

        private async void GetOptions(Button button)
        {
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請根據以下劇情給予可能的走向選擇，必須簡短至15字內\n以下是劇情:" + userInput + "\n請你回答:",
                Model = "text-davinci-003",
                MaxTokens = 2048
            });

            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = completionResponse.Choices[0].Text.Trim();
        }

        private async void SendImageRequest()
        {
            //image.sprite = null;
            //loadingLabel.SetActive(true);

            //
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請根據以下劇情給予應該生成的背景圖片生成詞\n劇情:「" + currentFullText + "」請直接給予生成詞:",
                Model = "text-davinci-003",
                MaxTokens = 2048
            });
            imgGenerateText = completionResponse.Choices[0].Text.Trim();
            Debug.Log(imgGenerateText);
            //

            var response = await openai.CreateImage(new CreateImageRequest
            {
                Prompt = imgGenerateText,
                Size = ImageSize.Size256
            });

            if (response.Data != null && response.Data.Count > 0)
            {
                using(var request = new UnityWebRequest(response.Data[0].Url))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                    request.SendWebRequest();

                    while (!request.isDone) await Task.Yield();

                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(request.downloadHandler.data);
                    var sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), Vector2.zero, 1f);
                    image.sprite = sprite;
                }
            }
            else
            {
                Debug.LogWarning("No image was created from this prompt.");
            }

            //loadingLabel.SetActive(false);
        }

        private void option4Act(){
            selfChoicing.SetActive(true);
            option1.interactable = false;
            option2.interactable = false;
            option3.interactable = false;
            option4.interactable = false;
        }
    }
}
