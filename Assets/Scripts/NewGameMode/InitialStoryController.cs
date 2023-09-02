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
    public class InitialStoryController : MonoBehaviour
    {
        private OpenAIApi openai = new OpenAIApi();

        [SerializeField] private ScrollRect scroll;
        [SerializeField] private GameObject initialStoryPanel;
        [SerializeField] private Button initialStoryPanelButton;
        [SerializeField] private Text initialStoryTextArea;
        [SerializeField] private GameObject initMoveOnTip;


        private string initialStory = "";
        private CancellationTokenSource token = new CancellationTokenSource();

        private void Start()
        {
            initialStoryPanelButton.interactable = false;
            initialStoryPanelButton.onClick.AddListener(InitialStoryPanelButtonAct);
            GetInitialStory();
        }

        private void GetInitialStory()
        {
            var initialStoryMessage = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "請為我生成一段武俠遊戲的初始劇情，遊戲目標是擊敗一個江湖中令人聞風喪膽的大反派，而我是一名初出茅廬的小子。請圍繞以上設定拓展出詳細的遊戲的初始介紹劇情，對於遊戲的世界觀和大反派的描述需要有更多細節。"
                }
            };
            void HandleResponse(List<CreateChatCompletionResponse> responses){
                scroll.verticalNormalizedPosition = 0;
                initialStoryTextArea.text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
            }
            void HandleComplete(){
                initMoveOnTip.SetActive(true);
                initialStoryPanelButton.interactable = true;
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = initialStoryMessage,
                Temperature = 1f,
                Stream = true
            }, HandleResponse, HandleComplete,token);
        }

        private void InitialStoryPanelButtonAct(){
            initialStoryPanel.SetActive(false);
        }
    }

}