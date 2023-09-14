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

        [SerializeField] private GameObject practiceSelectPanel;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Button enterPracticeButton;




        private string currentFullText = "";
        private string[] currentFullTexts = new string[50];
        private int textBoxCount = 0;
        private bool canMove = false;
        private float lastChangeTime;
        private int addForceValue = 0;
        private string practiceType = "";

        private void Start()
        {
            practiceButton.onClick.AddListener(EnterPracticeSelect);
            enterPracticeButton.onClick.AddListener(EnterPractice);
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

        private void EnterPracticeSelect(){
            practiceSelectPanel.SetActive(true);
        }
        public void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault(); // 獲取選中的Toggle
                if (selectedToggle != null)
                {
                    practiceType = selectedToggle.GetComponentInChildren<Text>().text; // 獲取選中Toggle的標籤文字
                    print(practiceType);
                }
            }
        }

        public void EnterPractice(){
            canMove = false;
            practiceSelectPanel.SetActive(false);
            practiceBackground.SetActive(true);

            int randomSoundInt = UnityEngine.Random.Range(2,15);
            AudioClip newSoundClip = Resources.Load<AudioClip>("GameMusic/WuXia/" + randomSoundInt); 
            practiceBackgroundSound.clip = newSoundClip;
            practiceBackgroundSound.enabled = true;
            practiceBackgroundSound.Play();

            addForceValue = UnityEngine.Random.Range(-1,11);
            string addCondition = "";
            switch (addForceValue)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    addCondition = "進步緩慢";
                    break;

                case 5:
                case 6:
                case 7:
                    addCondition = "進步普通";
                    break;

                case 8:
                case 9:
                case 10:
                    addCondition = "進步飛快";
                    break;
                default:
                    addCondition = "毫無進步";
                    break;
            }

            lastChangeTime = Time.time;
            var practiceStoryMessage = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "請以第一人稱視角生成一小段修練武功一天且" + addCondition + "的劇情，而你修練的武功是" + practiceType
                }
            };
            void HandleResponse(List<CreateChatCompletionResponse> responses){
                currentFullText = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = practiceStoryMessage,
                Temperature = 0.8f,
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
                    GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetForceValue(addForceValue);
                    FullStoryController.day++;
                }
            }
        }
    }

}