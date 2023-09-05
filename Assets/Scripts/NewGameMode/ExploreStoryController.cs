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
    public class ExploreStoryController : MonoBehaviour
    {
        private OpenAIApi openai = new OpenAIApi();
        private CancellationTokenSource token = new CancellationTokenSource();

        [SerializeField] private AudioSource exploreBackgroundSound;

        [SerializeField] private Button exploreButton;
        [SerializeField] private GameObject exploreBackground;
        [SerializeField] private Image exploreBackgroundImg;
        [SerializeField] private Button exploreBackgroundButton;
        [SerializeField] private Button textBoxButton;  
        
        [SerializeField] private Text exploreStoryTextArea;
        [SerializeField] private GameObject moveOnTip;


        private string currentFullText = "";
        private string[] currentFullTexts = new string[50];
        private int textBoxCount = 0;
        private bool canMove = false;
        private float lastChangeTime;

        private void Start()
        {
            exploreButton.onClick.AddListener(() => EnterExplore(0));
            textBoxButton.onClick.AddListener(MoveOn);
            exploreBackgroundButton.onClick.AddListener(MoveOn);
        }

        private void Update()
        {
            currentFullTexts = currentFullText.Split("\n");
            if(textBoxCount >= 0 && textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] != null){
                //監測字串變化
                if(exploreStoryTextArea.text != currentFullTexts[textBoxCount]){
                    exploreStoryTextArea.text = currentFullTexts[textBoxCount];
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

        public void EnterExplore(int num){
            canMove = false;
            int placeNum = 0;
            string placeName = "";
            if(num == 0){
                int randomIndex = UnityEngine.Random.Range(0,FullStoryController.randomPlaces.Count);
                placeNum = FullStoryController.randomPlaces[randomIndex];
            }else{
                placeNum = num;
            }
            placeName = FullStoryController.GetPlaceNameByNum(placeNum);        

            int randomInt = UnityEngine.Random.Range(1,5);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + placeNum + "/" + randomInt);
            exploreBackgroundImg.sprite = newSprite;
            exploreBackground.SetActive(true);

            int randomSoundInt = UnityEngine.Random.Range(1,15);
            AudioClip newSoundClip = Resources.Load<AudioClip>("GameMusic/WuXia/" + randomSoundInt); 
            exploreBackgroundSound.clip = newSoundClip;
            exploreBackgroundSound.enabled = true;
            exploreBackgroundSound.Play();

            lastChangeTime = Time.time;
            var exploreStoryMessage = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "請給我一小段武俠世界故事的探險劇情，你是一個武俠世界中的小夥子，以第一人稱視角探索" + placeName + "這個區域。"
                }
            };
            void HandleResponse(List<CreateChatCompletionResponse> responses){
                currentFullText = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = exploreStoryMessage,
                Temperature = 1f,
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
                    exploreBackground.SetActive(false);
                    FullStoryController.day++;
                }
            }
        }
    }

}