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
        private int addValueTypeNum = 0;
        private int addValueAmont = 0;

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
            addValueTypeNum = UnityEngine.Random.Range(1,5);
            string addValueType = "";
            switch(addValueTypeNum){
                case 1:
                    addValueType = "武功";
                    break;
                case 2:
                    addValueType = "智慧";
                    break;
                case 3:
                    addValueType = "情報";
                    break;
                case 4:
                    addValueType = "名聲";
                    break;
            }
            addValueAmont = UnityEngine.Random.Range(-1,11);
            string addCondition = "";
            switch (addValueAmont)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    addCondition = "些微增長";
                    break;

                case 5:
                case 6:
                case 7:
                    addCondition = "有所增長";
                    break;

                case 8:
                case 9:
                case 10:
                    addCondition = "增長極多";
                    break;
                default:
                    addCondition = "毫無增長";
                    break;
            }


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
                    Content = "請給我一小段武俠世界故事的探險劇情，以第一人稱視角探索" + placeName + "這個區域，並且這次的探險會使你的" + addValueType + addCondition
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
                    switch(addValueTypeNum){
                        case 1:
                            GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetForceValue(addValueAmont);
                            break;
                        case 2:
                            GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetWisdomValue(addValueAmont);
                            break;
                        case 3:
                            GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetInfoValue(addValueAmont);
                            break;
                        case 4:
                            GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetFameValue(addValueAmont);
                            break;
                    }
                    exploreBackground.SetActive(false);
                    FullStoryController.day++;
                }
            }
        }
    }

}