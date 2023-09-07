using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;



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

        private CanvasGroup initPanelCanvasGroup;
        private float fadeDuration = 3.0f;


        public static int plotIndex;
        private string[] plotsPrompts = new string[10];
        private string plotPrompt;

        private string initialStory = "";
        private CancellationTokenSource token = new CancellationTokenSource();

        private void Start()
        {
            InitPlotPrompt();
            initPanelCanvasGroup = initialStoryPanel.GetComponent<CanvasGroup>();
            initPanelCanvasGroup.alpha = 1;
            //initialStoryPanelButton.interactable = false;
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
                    Content = "請為我生成一段武俠遊戲的初始劇情，遊戲目標是" + plotPrompt +"。請圍繞以上設定拓展出詳細的遊戲的初始介紹劇情，對於遊戲的世界觀和背景設定、劇情的描述需要有更多細節，並在最後告知玩家他需要在有限的時間內為遊戲目標做準備:\n\n"
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
                Temperature = 0.7f,
                Stream = true
            }, HandleResponse, HandleComplete,token);
        }

        private void InitialStoryPanelButtonAct(){
            StartCoroutine(InitFadeOut());
        }

        private IEnumerator InitFadeOut()
        {
            float elapsedTime = 0;

            while (elapsedTime < fadeDuration)
            {
                initPanelCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 確保透明度達到1
            initPanelCanvasGroup.alpha = 0;
            initialStoryPanel.SetActive(false);
        }

        private void InitPlotPrompt(){
            plotsPrompts[0] = "在遊戲最後參與武林大會擊敗所有人成為第一名";
            plotsPrompts[1] = "在遊戲最後成為朝廷的官員";
            plotsPrompts[2] = "在遊戲最後找到仇人並擊敗他成功復仇";
            plotsPrompts[3] = "在遊戲最後找友人解救他並擊敗綁架他的人";
            plotsPrompts[4] = "在遊戲最後擊敗所有勢力";
            plotsPrompts[5] = "在遊戲最後創立自己的門派";
            plotsPrompts[6] = "在遊戲最後獨自擊敗邪惡";
            plotsPrompts[7] = "在遊戲最後組織各方勢力擊敗邪惡";
            plotsPrompts[8] = "在遊戲最後擊敗外敵";
            plotsPrompts[9] = "在遊戲最後解開層層困難獲得寶藏";

            plotIndex = UnityEngine.Random.Range(0,10);
            plotPrompt = plotsPrompts[plotIndex];
            print(plotPrompt);
        }
    }

}