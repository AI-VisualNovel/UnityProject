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
        [SerializeField] private GameObject defaultChoicing;
        [SerializeField] private Button button;
        [SerializeField] private Text textArea;
        [SerializeField] private Image image;

        [SerializeField] private Button option1;
        [SerializeField] private Button option2;
        [SerializeField] private Button option3;

        
        private OpenAIApi openai = new OpenAIApi(InputFieldManager.user_api);
        
        private string userInput;
        private string Instruction ;

        private string currentFullText = "";
        private string remainingText;

        private void Start()
        {           

            defaultChoicing.SetActive(false);

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
                Instruction = "你現在是一個遊戲終端，根據我的指示生成劇情發展。\n產出的圖像需要是"+gameStyle+"的樣子，故事的主題是一種"+gameMode+"的背景\n同時我希望遊戲是朝向"+gameDirection+"Q: ";
            }
            else
            {
                Instruction = "You are now acting as a game terminal, generate plot development according to my instructions. \nQ: ";
            }
            button.onClick.AddListener(SendReply);
            option1.onClick.AddListener(() => SendReply2(option1));
            option2.onClick.AddListener(() => SendReply2(option2));
            option3.onClick.AddListener(() => SendReply2(option3));

            Debug.Log(gameMode+gameStyle+gamePicQuality+gameDirection+gameLanguage);
            remainingText = currentFullText;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0))
            {
                MoveOn();
            }
        }
        
        private async void SendReply()
        {
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
            //SendImageRequest();

            Debug.Log(currentFullText);
            int charactersToAdd = Mathf.Min(113, remainingText.Length);
            string displayedText = remainingText.Substring(0, charactersToAdd);
            textArea.text = displayedText;
            remainingText = remainingText.Remove(0, charactersToAdd);

            Instruction += $"{completionResponse.Choices[0].Text}\nQ: ";
            
            }
            catch(Exception ex)
            {
                WrongApiPanel.SetActive(true);
            }
            
        }

        private async void SendReply2(Button button)
        {
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
            //SendImageRequest();

            Debug.Log(currentFullText);
            int charactersToAdd = Mathf.Min(113, remainingText.Length);
            string displayedText = remainingText.Substring(0, charactersToAdd);
            textArea.text = displayedText;
            remainingText = remainingText.Remove(0, charactersToAdd);

            Instruction += $"{completionResponse.Choices[0].Text}\nQ: ";
            
            }
            catch(Exception ex)
            {
                WrongApiPanel.SetActive(true);
            }
            
        }

        private void MoveOn(){
            if(remainingText.Length > 0){
                int charactersToAdd = Mathf.Min(113, remainingText.Length);
                string displayedText = remainingText.Substring(0, charactersToAdd);
                textArea.text = displayedText;
                remainingText = remainingText.Remove(0, charactersToAdd);
            }else{
                optionChoicing.SetActive(true);
                defaultChoicing.SetActive(true);
            }
        }

        private async void GetOptions(Button button)
        {
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請根據以下劇情給予可能的走向選擇，必須簡短至15字內\n範例:\n劇情:小明來到一個岔路口，請問他該怎麼做?\n你要回答:向左走\n以下是劇情:" + userInput + "請你回答:",
                Model = "text-davinci-003",
                MaxTokens = 2048
            });

            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = completionResponse.Choices[0].Text.Trim();
        }

        private async void SendImageRequest()
        {
            image.sprite = null;
            //loadingLabel.SetActive(true);

            var response = await openai.CreateImage(new CreateImageRequest
            {
                Prompt = currentFullText,
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
