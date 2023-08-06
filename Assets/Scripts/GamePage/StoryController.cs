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
    public class StoryController : MonoBehaviour
    {        
        [SerializeField] private GameObject WrongApiPanel;
        [SerializeField] private InputField inputField;
        [SerializeField] private GameObject optionChoicing;
        //[SerializeField] private GameObject defaultChoicing;
        //[SerializeField] private GameObject gameUI;
        [SerializeField] private Button button;
        [SerializeField] private Text textArea;
        [SerializeField] private Image image;

        [SerializeField] private Button option1;
        [SerializeField] private Button option2;
        [SerializeField] private Button option3;

        //[SerializeField] private Button hideUI;

        private OpenAIApi openai = new OpenAIApi(InputFieldManager.user_api);
        
        private string userInput;
        private string Instruction ;

        private string currentFullText = "";
        private string remainingText;
        private string imgGenerateText = "";

        private bool optionShow = false;
        private bool UIHide = false;

        private void Start()
        {           
            optionChoicing.SetActive(false);

            // CreateNewGame Variable
            string gameMode,gameStyle,gamePicQuality,gameDirection,gameLanguage;
            if(CreateNewGameButton.gamedir==null)
            {
                gameMode = CreateNewGameButton.buttonTexts[0];
                gameStyle = CreateNewGameButton.buttonTexts[1];
                gamePicQuality = CreateNewGameButton.buttonTexts[2];
                gameDirection = CreateNewGameButton.buttonTexts[3];
                gameLanguage = CreateNewGameButton.buttonTexts[4];
            }
            else
            {
                gameDirection = CreateNewGameButton.gamedir;
                gameMode = CreateNewGameButton.buttonTexts[0];
                gameStyle = CreateNewGameButton.buttonTexts[1];
                gamePicQuality = CreateNewGameButton.buttonTexts[2];
                gameLanguage = CreateNewGameButton.buttonTexts[3];
            }
            if(gameLanguage=="中文")
            {
                Instruction = "我現在要跟你玩文字遊戲。故事背景設定在台灣的"+gameMode+"，請確認好資訊無誤再放入故事中，遊玩視角為第二人稱。請詳細敘述主角目前的所在地、場景、正在發生什麼事情、會聽到、看到什麼東西、對建築物的描述也請符合"+gameMode+"，當我問出有關當時造就的情況的問題時，請以正確的資訊教導我。首先請生成150字的故事開頭，第一句話以:你是 {主角名字}，{身分} ,開頭，之後以第二人稱視角敘述周遭環境，必要時也可以以旁白角度描寫事件發生經過、場景描述等。之後我會根據劇情輸入主角（我）後續的動作，再依照我的輸入產生出下一個篇幅為50~100字的劇情，繼續引導故事伏筆前進，貼近當時的歷史背景，適時給我一些線索去探索，盡量在回覆的結尾拋給我一個問題，最後預設一個結尾，引導我到結尾即遊戲結束";
            }
            else
            {
                Instruction = "我現在要跟你玩文字遊戲。故事背景設定在台灣的"+gameMode+"，請確認好資訊無誤再放入故事中，遊玩視角為第二人稱。請詳細敘述主角目前的所在地、場景、正在發生什麼事情、會聽到、看到什麼東西、對建築物的描述也請符合"+gameMode+"，當我問出有關當時造就的情況的問題時，請以正確的資訊教導我。首先請生成150字的故事開頭，第一句話以:你是 {主角名字}，{身分} ,開頭，之後以第二人稱視角敘述周遭環境，必要時也可以以旁白角度描寫事件發生經過、場景描述等。之後我會根據劇情輸入主角（我）後續的動作，再依照我的輸入產生出下一個篇幅為50~100字的劇情，繼續引導故事伏筆前進，貼近當時的歷史背景，適時給我一些線索去探索，盡量在回覆的結尾拋給我一個問題，最後預設一個結尾，引導我到結尾即遊戲結束";
                //Instruction = "You are now acting as a game terminal, generate plot development according to my instructions. \nQ: ";ㄈ
                Debug.Log("123");
            }

            SendReply();
            button.onClick.AddListener(SendReply);
            option1.onClick.AddListener(() => SendReplyButton(option1));
            option2.onClick.AddListener(() => SendReplyButton(option2));
            option3.onClick.AddListener(() => SendReplyButton(option3));
            //hideUI.onClick.AddListener(UIHiding);


            Debug.Log(gameMode+gameStyle+gamePicQuality+gameDirection+gameLanguage);
            remainingText = currentFullText;
        }

        // private void Update() {
        //     if (Input.GetMouseButtonDown(0)){
        //         if(UIHide == true){
        //             gameUI.SetActive(true);
        //             UIHide = false;
        //         }else{
        //             MoveOn();
        //         }
        //     }
        // }
        
        // private void MoveOn(){
        //         if(remainingText.Length > 0){
        //             int charactersToAdd = Mathf.Min(113, remainingText.Length);
        //             string displayedText = remainingText.Substring(0, charactersToAdd);
        //             textArea.text = displayedText;
        //             remainingText = remainingText.Remove(0, charactersToAdd);
        //         }else if(optionShow == true){
        //             optionChoicing.SetActive(true);
        //     }
        // }

        // private void UIHiding(){
        //     gameUI.SetActive(false);
        //     UIHide = true;
        // }

        private async void SendReply()
        {
            optionShow = false;
            try{
            userInput = inputField.text;
            
            Instruction += $"{inputField.text}\nA: ";

            textArea.text = "...";
            inputField.text = "";

            optionChoicing.SetActive(false);

            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = Instruction,
                Model = "text-davinci-003",
                MaxTokens = 2048
            });

            currentFullText = completionResponse.Choices[0].Text.Trim();
            remainingText = currentFullText;

            GetOptions(option1);
            GetOptions(option2);
            GetOptions(option3);
            SendImageRequest();

            Debug.Log(currentFullText);
            int charactersToAdd = Mathf.Min(113, remainingText.Length);
            string displayedText = remainingText.Substring(0, charactersToAdd);
            textArea.text = displayedText;
            remainingText = remainingText.Remove(0, charactersToAdd);

            Instruction += $"{completionResponse.Choices[0].Text}\nQ: ";
            
            optionShow = true;
            }
            catch(Exception ex)
            {
                WrongApiPanel.SetActive(true);
            }
            
        }

        private async void SendReplyButton(Button button)
        {
            optionShow = false;
            try{
            userInput = button.GetComponentInChildren<Text>().text;
            
            Instruction += $"{inputField.text}\nA: ";

            textArea.text = "...";
            inputField.text = "";

            optionChoicing.SetActive(false);

            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = Instruction,
                Model = "text-davinci-003",
                MaxTokens = 2048
            });

            currentFullText = completionResponse.Choices[0].Text.Trim();
            remainingText = currentFullText;

            GetOptions(option1);
            GetOptions(option2);
            GetOptions(option3);
            SendImageRequest();

            Debug.Log(currentFullText);
            int charactersToAdd = Mathf.Min(113, remainingText.Length);
            string displayedText = remainingText.Substring(0, charactersToAdd);
            textArea.text = displayedText;
            remainingText = remainingText.Remove(0, charactersToAdd);
            
            Instruction += $"{completionResponse.Choices[0].Text}\nQ: ";
            
            optionShow = true;
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
    }
}

