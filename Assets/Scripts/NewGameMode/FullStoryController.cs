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
    public class FullStoryController : MonoBehaviour
    {
        private OpenAIApi openai = new OpenAIApi();
        private CancellationTokenSource token = new CancellationTokenSource();

        public static int day = 1;
        public static int CE = 5;

        [SerializeField] private Text dayCounter;
        [SerializeField] private Text initialStoryTextArea;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private GameObject endStoryPanel;
        [SerializeField] private Text endStoryTextArea;


        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Test();
            }

            dayCounter.text = day.ToString();
            //!!!!!!!!!!!!!!小心使用!!!!!!!!!!!!!!!!!!!
            // if(day >= 2){
            //     EnterTheEnd();
            //     day = -1;
            // }
            //!!!!!!!!!!!!!!小心使用!!!!!!!!!!!!!!!!!!!
        }

        private void Test(){
            print(CE);
        }

        private void EnterTheEnd()
        {
            string theEndPrompt = "";
            if(CE >= 100){
                theEndPrompt = "最後我成功擊敗了反派";
            }else if(CE <= 20){
                theEndPrompt = "最後我被反派擊敗";
            }else{
                theEndPrompt = "最後我一事無成";
            }

            endStoryPanel.SetActive(true);
            var endStoryMessage = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "assistant",
                    Content = initialStoryTextArea.text
                },
                new ChatMessage()
                {
                    Role = "user",
                    Content = theEndPrompt + "，請依此生成相應劇情"
                }
            };
            void HandleResponse(List<CreateChatCompletionResponse> responses){
                scroll.verticalNormalizedPosition = 0;
                endStoryTextArea.text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = endStoryMessage,
                Temperature = 1f,
                Stream = true
            }, HandleResponse, null,token);
        }
    }

}
