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


        [SerializeField] private Button exploreButton;
        [SerializeField] private GameObject exploreBackground;
        [SerializeField] private Image exploreBackgroundImg;
        [SerializeField] private Button exploreBackgroundButton;
        [SerializeField] private Button textBoxButton;  
        
        [SerializeField] private Text exploreStoryTextArea;
        [SerializeField] private GameObject moveOnTip;


        private string currentFullText = "";
        private string[] currentFullTexts = new string[80];
        private int textBoxCount = 0;
        private bool canMove = false;
        private float lastChangeTime;


        private void Start()
        {
            exploreButton.onClick.AddListener(EnterExplore);
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

        private void EnterExplore(){
            int randomPlaceNum = UnityEngine.Random.Range(1,31);
            string randomPlace = "";
            switch (randomPlaceNum)
            {
                case 1:
                    randomPlace = "小村莊";
                    break;
                case 2:
                    randomPlace = "山谷";
                    break;
                case 3:
                    randomPlace = "山洞";
                    break;
                case 4:
                    randomPlace = "寺廟";
                    break;
                case 5:
                    randomPlace = "遺跡廢墟";
                    break;
                case 6:
                    randomPlace = "竹林";
                    break;
                case 7:
                    randomPlace = "瀑布";
                    break;
                case 8:
                    randomPlace = "荒野";
                    break;
                case 9:
                    randomPlace = "市集";
                    break;
                case 10:
                    randomPlace = "擂台";
                    break;
                case 11:
                    randomPlace = "酒樓";
                    break;
                case 12:
                    randomPlace = "客棧";
                    break;
                case 13:
                    randomPlace = "武學門派";
                    break;
                case 14:
                    randomPlace = "武林聚會";
                    break;
                case 15:
                    randomPlace = "城牆";
                    break;
                case 16:
                    randomPlace = "山寨";
                    break;
                case 17:
                    randomPlace = "密室";
                    break;
                case 18:
                    randomPlace = "山谷涼亭";
                    break;
                case 19:
                    randomPlace = "山間小徑";
                    break;
                case 20:
                    randomPlace = "軍營";
                    break;
                case 21:
                    randomPlace = "冰川";
                    break;
                case 22:
                    randomPlace = "沙漠";
                    break;
                case 23:
                    randomPlace = "皇宮";
                    break;
                case 24:
                    randomPlace = "雪山";
                    break;
                case 25:
                    randomPlace = "森林";
                    break;
                case 26:
                    randomPlace = "港口";
                    break;
                case 27:
                    randomPlace = "湖泊";
                    break;
                case 28:
                    randomPlace = "衙門";
                    break;
                case 29:
                    randomPlace = "戰場";
                    break;
                case 30:
                    randomPlace = "懸崖";
                    break;
                default:
                    randomPlace = "未知地點";
                    break;
            }
            int randomInt = UnityEngine.Random.Range(1,5);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + randomPlaceNum + "/" + randomInt);
            exploreBackgroundImg.sprite = newSprite;
            exploreBackground.SetActive(true);

            lastChangeTime = Time.time;
            var exploreStoryMessage = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "請給我一小段武俠世界故事的探險劇情，你是一個武俠世界中的小夥子，以第一人稱視角探索" + randomPlace + "這個區域。"
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
                if(textBoxCount >= currentFullTexts.Length){
                    canMove = false;
                    textBoxCount = 0;
                    exploreBackground.SetActive(false);
                }
            }
        }
    }

}