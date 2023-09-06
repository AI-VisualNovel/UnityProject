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
    public class PracticeStoryController : MonoBehaviour
    {
        private OpenAIApi openai = new OpenAIApi();
        private CancellationTokenSource token = new CancellationTokenSource();

        [SerializeField] private AudioSource practiceBackgroundSound;

        [SerializeField] private Button practiceButton;
        [SerializeField] private GameObject practiceBackground;
        [SerializeField] private Image practiceBackgroundImg;
        [SerializeField] private Button practiceBackgroundButton;
        [SerializeField] private Button textBoxButton;  
        
        [SerializeField] private Text practiceStoryTextArea;
        [SerializeField] private GameObject moveOnTip;


        private string currentFullText = "";
        private string[] currentFullTexts = new string[50];
        private int textBoxCount = 0;
        private bool canMove = false;
        private float lastChangeTime;

        private void Start()
        {
            practiceButton.onClick.AddListener(() => EnterPractice(0));
            textBoxButton.onClick.AddListener(MoveOn);
            practiceBackgroundButton.onClick.AddListener(MoveOn);
        }

        private void Update()
        {
            currentFullTexts = currentFullText.Split("\n");
            if(textBoxCount >= 0 && textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] != null){
                //監測字串變化
                if(practiceStoryTextArea.text != currentFullTexts[textBoxCount]){
                    practiceStoryTextArea.text = currentFullTexts[textBoxCount];
                    lastChangeTime = Time.time;
                    canMove = false;
                }
                
                if(Time.time - lastChangeTime >= 1f && !canMove){
                    canMove = true;
                }
            }

            if(canMove){
                moveOnTip.SetActive(true);
            }else{
                moveOnTip.SetActive(false);
            }
        }

        public void EnterPractice(int num){
            canMove = false;
            practiceBackground.SetActive(true);

            int randomSoundInt = UnityEngine.Random.Range(1,15);
            AudioClip newSoundClip = Resources.Load<AudioClip>("GameMusic/WuXia/" + randomSoundInt); 
            practiceBackgroundSound.clip = newSoundClip;
            practiceBackgroundSound.enabled = true;
            practiceBackgroundSound.Play();

            lastChangeTime = Time.time;
            var practiceStoryMessage = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "請以第一人稱視角生成一小段修練武功一天的劇情"
                }
            };
            void HandleResponse(List<CreateChatCompletionResponse> responses){
                currentFullText = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = practiceStoryMessage,
                Temperature = 0.5f,
                Stream = true
            }, HandleResponse, null,token);
        }

        private void MoveOn(){
            if(canMove){
                textBoxCount++;
                if(textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] == ""){
                    textBoxCount++;
                }
                if(textBoxCount != 0 && textBoxCount >= currentFullTexts.Length){
                    canMove = false;
                    textBoxCount = 0;
                    practiceBackground.SetActive(false);
                    GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetForceValue();
                    FullStoryController.day++;
                }
            }
        }
    }

}