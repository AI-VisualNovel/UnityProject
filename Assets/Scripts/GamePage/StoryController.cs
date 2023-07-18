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
        [SerializeField] private InputField inputField;
        [SerializeField] private GameObject optionChoicing;
        [SerializeField] private Button button;
        [SerializeField] private Text textArea;
        [SerializeField] private Image image;
        
        private OpenAIApi openai = new OpenAIApi(InputFieldManager.user_api);
        
        private string userInput;
        private string Instruction = "You are now acting as a game terminal, generate plot development according to my instructions. \nQ: ";
        private string currentFullText = "";
        private string remainingText;

        private void Start()
        {
            button.onClick.AddListener(SendReply);
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

            SendImageRequest();

            Debug.Log(currentFullText);
            int charactersToAdd = Mathf.Min(113, remainingText.Length);
            string displayedText = remainingText.Substring(0, charactersToAdd);
            textArea.text = displayedText;
            remainingText = remainingText.Remove(0, charactersToAdd);

            Instruction += $"{completionResponse.Choices[0].Text}\nQ: ";
        }

        private void MoveOn(){
            if(remainingText.Length > 0){
                int charactersToAdd = Mathf.Min(113, remainingText.Length);
                string displayedText = remainingText.Substring(0, charactersToAdd);
                textArea.text = displayedText;
                remainingText = remainingText.Remove(0, charactersToAdd);
            }else{
                optionChoicing.SetActive(true);
            }
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
