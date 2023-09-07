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
    public class InitialStoryControllerFantasy : MonoBehaviour
    {
        private OpenAIApi openai = new OpenAIApi();

        [SerializeField] private ScrollRect scroll;
        [SerializeField] private GameObject initialStoryPanel;
        [SerializeField] private Button initialStoryPanelButton;
        [SerializeField] private Text initialStoryTextArea;
        [SerializeField] private GameObject initMoveOnTip;


        public int plotIndex;
        private string[] plotsPrompts = new string[10];
        private string plotPrompt;

        private string initialStory = "";
        private CancellationTokenSource token = new CancellationTokenSource();

        private void Start()
        {
            InitPlotPrompt();
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
                    Content = "請為我生成一段奇幻遊戲的初始劇情，遊戲目標是" + plotPrompt +"。請圍繞以上設定拓展出詳細的遊戲的初始介紹劇情，對於遊戲的世界觀和背景設定、劇情的描述需要有更多細節，並在最後告知玩家他需要在有限的時間內為遊戲目標做準備:\n\n"
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
                Temperature = 0.0f,
                Stream = true
            }, HandleResponse, HandleComplete,token);
        }

        private void InitialStoryPanelButtonAct(){
            initialStoryPanel.SetActive(false);
        }

        private void InitPlotPrompt(){
            plotsPrompts[0] = "在遊戲最後學會所有魔法成為魔法大師";
            plotsPrompts[1] = "在遊戲最後成為魔法教師";
            plotsPrompts[2] = "在遊戲最後找到愛人並與他結婚";
            plotsPrompts[3] = "在遊戲最後找到朋友並與他一同修練";
            plotsPrompts[4] = "在遊戲最後成為魔法最厲害的人";
            plotsPrompts[5] = "在遊戲最後創立自己專屬法術";
            plotsPrompts[6] = "在遊戲最後獨自擊敗邪惡";
            plotsPrompts[7] = "在遊戲最後組成各個組織擊敗邪惡";
            plotsPrompts[8] = "在遊戲最後擊敗外敵";
            plotsPrompts[9] = "在遊戲最後解開層層難關獲得寶藏";

            plotIndex = UnityEngine.Random.Range(0,10);
            plotPrompt = plotsPrompts[plotIndex];
        }
    }

}