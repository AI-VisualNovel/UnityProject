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

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

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

        private void Start()
        {
            backgroundButton.onClick.AddListener(MoveOn);
            textBoxButton.onClick.AddListener(MoveOn);
            sendButton.onClick.AddListener(SendReply);
            SendReply();
        }

        private void Update(){
            print(textBoxCount);
            if(currentMessageRec && suspend == false){
                currentFullTexts = currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text.Split("\n");
            }
            if(textBoxCount >= 0 && textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] != null && suspend == false){
                textArea.text = currentFullTexts[textBoxCount];
            }
        }

        private async void SendReply()
        {
            try{
                textBoxCount = 0;
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
                messages.Add(recMessage);

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

                inputField.enabled = true;
                sendButton.enabled = true;
            }catch(Exception ex){
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }

        private void MoveOn(){
            suspend = false;
            textBoxCount++;
            if(textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] == ""){
                textBoxCount++;
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
    }
}

