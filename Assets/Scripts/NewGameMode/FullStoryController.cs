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
using UnityEngine.SceneManagement;


namespace OpenAI
{
    public class FullStoryController : MonoBehaviour
    {
        private OpenAIApi openai = new OpenAIApi();
        private CancellationTokenSource token = new CancellationTokenSource();

        public static int day = 1;

        [SerializeField] private Text dayCounter;
        [SerializeField] private Text initialStoryTextArea;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private GameObject endStoryPanel;
        [SerializeField] private Text endStoryTextArea;
        [SerializeField] private List<Button> placeButtons;
        [SerializeField] private List<Image> placeImages;
        [SerializeField] private List<GameObject> placeImagesObj;
        [SerializeField] private List<RectTransform> npcPrefrebs;
        [SerializeField] private List<Transform> npcPlaces;
        [SerializeField] private GameObject endMoveOnTip;

        ExploreStoryController exploreController;

        private List<int> allPlaces = new List<int>();
        private List<int> fixedPlaces = new List<int>();
        private List<int> fixedPlacesImgNum = new List<int>();
        public static List<int> randomPlaces = new List<int>();


        private CanvasGroup endPanelCanvasGroup;
        private float fadeDuration = 4.0f;

        private void Start()
        {
            //分配地圖
            for (int i = 1; i <= 30; i++){
                allPlaces.Add(i);
            }
            for (int i = 0; i < 10; i++){
                int randomIndex = UnityEngine.Random.Range(0, allPlaces.Count);
                int randomNumber = allPlaces[randomIndex];
                fixedPlaces.Add(randomNumber);
                allPlaces.RemoveAt(randomIndex);

                int randomImgIndex = UnityEngine.Random.Range(1,5);
                Sprite newSprite = Resources.Load<Sprite>("WuxiaBackground/" + fixedPlaces[i] + "/" + randomImgIndex);
                placeImages[i].sprite = newSprite;

                // int randomNpcNum = UnityEngine.Random.Range(0,2);
                // if(randomNpcNum == 1){
                    NpcGenerator(npcPlaces[i],fixedPlaces[i]);
                //}
            }
            randomPlaces.AddRange(allPlaces);
            exploreController = GetComponent<ExploreStoryController>();
            for (int i = 0; i < placeButtons.Count; i++)
            {
                int placeIndex = i;
                placeButtons[i].GetComponentInChildren<Text>().text = GetPlaceNameByNum(fixedPlaces[i]);
                placeButtons[i].onClick.AddListener(() => FixedPlaceButtonAct(placeIndex));
            }

            endPanelCanvasGroup = endStoryPanel.GetComponent<CanvasGroup>();
            endPanelCanvasGroup.alpha = 0;
            endStoryPanel.GetComponent<Button>().onClick.AddListener(BackToMainPage);
            endStoryPanel.GetComponent<Button>().interactable = false;

            GetRandomNewPlaceName();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Test();
            }

            dayCounter.text = day.ToString();
            //!!!!!!!!!!!!!!小心使用!!!!!!!!!!!!!!!!!!!
            if(day >= 3){
                Invoke("EnterTheEnd", 3.0f);
                day = -1;
            }if(day == -1){
                dayCounter.text = "3";
            }
            //!!!!!!!!!!!!!!小心使用!!!!!!!!!!!!!!!!!!!
        }

        private void Test(){
            //print(CE);
        }

        public static string GetPlaceNameByNum(int num){
            string placeName = "";
            switch (num)
            {
                case 1:
                    placeName = "小村莊";
                    break;
                case 2:
                    placeName = "山谷";
                    break;
                case 3:
                    placeName = "山洞";
                    break;
                case 4:
                    placeName = "寺廟";
                    break;
                case 5:
                    placeName = "遺跡廢墟";
                    break;
                case 6:
                    placeName = "竹林";
                    break;
                case 7:
                    placeName = "瀑布";
                    break;
                case 8:
                    placeName = "荒野";
                    break;
                case 9:
                    placeName = "市集";
                    break;
                case 10:
                    placeName = "擂台";
                    break;
                case 11:
                    placeName = "酒樓";
                    break;
                case 12:
                    placeName = "客棧";
                    break;
                case 13:
                    placeName = "武學門派";
                    break;
                case 14:
                    placeName = "武林聚會";
                    break;
                case 15:
                    placeName = "城牆";
                    break;
                case 16:
                    placeName = "山寨";
                    break;
                case 17:
                    placeName = "密室";
                    break;
                case 18:
                    placeName = "山谷涼亭";
                    break;
                case 19:
                    placeName = "山間小徑";
                    break;
                case 20:
                    placeName = "軍營";
                    break;
                case 21:
                    placeName = "冰川";
                    break;
                case 22:
                    placeName = "沙漠";
                    break;
                case 23:
                    placeName = "皇宮";
                    break;
                case 24:
                    placeName = "雪山";
                    break;
                case 25:
                    placeName = "森林";
                    break;
                case 26:
                    placeName = "港口";
                    break;
                case 27:
                    placeName = "湖泊";
                    break;
                case 28:
                    placeName = "衙門";
                    break;
                case 29:
                    placeName = "戰場";
                    break;
                case 30:
                    placeName = "懸崖";
                    break;
                default:
                    placeName = "未知地點";
                    break;
            }
            return placeName;
        } 

        private async void GetRandomNewPlaceName(){
            for (int i = 0; i < placeButtons.Count; i++)
            {
                string placeName = placeButtons[i].GetComponentInChildren<Text>().text;
                var getNewPlaceNameMessage = new List<ChatMessage>
                {
                    new ChatMessage()
                    {
                        Role = "user",
                        Content = "請給予" + placeName + "這種地方一個名字，五個字以內\n\n請直接回答地方的名字:\n"
                    }
                };
                var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
                {
                    Model = "gpt-3.5-turbo-0613",
                    Messages = getNewPlaceNameMessage,
                    MaxTokens = 128,
                    Temperature = 1.0f
                });

                placeButtons[i].GetComponentInChildren<Text>().text = completionResponse.Choices[0].Message.Content.Trim();
                print(completionResponse.Choices[0].Message.Content.Trim());
            }
        }

        public static string GetNpcTypeByNum(int num){
           string npcType = "";
            switch (num)
            {
                case 1:
                    npcType = "Monk";
                    break;
                case 2:
                    npcType = "Swordsman";
                    break;
                case 3:
                    npcType = "Nun";
                    break;
                case 4:
                    npcType = "Bandits";
                    break;
                case 5:
                    npcType = "Heroine";
                    break;
                case 6:
                    npcType = "Taoist";
                    break;
                case 7:
                    npcType = "Beggars";
                    break;
                case 8:
                    npcType = "Catch";
                    break;
                case 9:
                    npcType = "Scholar";
                    break;
                case 10:
                    npcType = "Knifesman";
                    break;
                case 11:
                    npcType = "MiddleagedKnight";
                    break;
                case 12:
                    npcType = "OldKnight";
                    break;
                case 13:
                    npcType = "OldMonk";
                    break;
                case 14:
                    npcType = "MysteriousAssassin";
                    break;
                case 15:
                    npcType = "DisciplesOfTheSwordSect";
                    break;
                case 16:
                    npcType = "DisciplesOfTheBoxingSect";
                    break;
                case 17:
                    npcType = "Officer";
                    break;
                case 18:
                    npcType = "Drunkard";
                    break;
                case 19:
                    npcType = "Vendors";
                    break;
                case 20:
                    npcType = "GrandMaster";
                    break;
                default:
                    npcType = "";
                    break;
            }
            return npcType;
        }
        public void FixedPlaceButtonAct(int placeIndex){
            placeImagesObj[placeIndex].SetActive(true);
        }

        private void NpcGenerator(Transform npcPlace,int placeNum){
            int randomNpcTypeNum = UnityEngine.Random.Range(1,21);
            var newNpc = Instantiate(npcPrefrebs[randomNpcTypeNum-1], npcPlace);

            string npcType = GetNpcTypeByNum(randomNpcTypeNum);
            int randomImgIndex = UnityEngine.Random.Range(1,5);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaNPC/" + npcType + "/"+ randomImgIndex);

            if (newSprite != null) {
                newNpc.GetChild(0).GetComponentInChildren<Image>().sprite = newSprite;
            }

            newNpc.GetComponent<NpcButtonController>().placeNum = placeNum;
        }

        private void EnterTheEnd()
        {
            string theEndPrompt = GameObject.Find("StoryController").GetComponent<GrowthSystemController>().EndJudgment(InitialStoryController.plotIndex);
            print(theEndPrompt);

            endStoryPanel.SetActive(true);
            StartCoroutine(FadeInEnd());
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
                    Content = "最後我" + theEndPrompt + "，請依此生成相應的結局劇情，劇情不可以在有轉折，僅僅依照 " + theEndPrompt + " 寫得更具體"
                }
            };
            void HandleResponse(List<CreateChatCompletionResponse> responses){
                scroll.verticalNormalizedPosition = 0;
                endStoryTextArea.text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
            }
            void HandleComplete(){
                endMoveOnTip.SetActive(true);
                endStoryPanel.GetComponent<Button>().interactable = true;
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = endStoryMessage,
                Temperature = 1f,
                Stream = true
            }, HandleResponse, HandleComplete,token);
        }
            private IEnumerator FadeInEnd()
            {
                float elapsedTime = 0;

                while (elapsedTime < fadeDuration)
                {
                    endPanelCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // 確保透明度達到1
                endPanelCanvasGroup.alpha = 1;
            }

            private void BackToMainPage(){
                SceneManager.LoadScene("MainPage");
            }

    }

}
