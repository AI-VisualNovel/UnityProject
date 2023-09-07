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
        [SerializeField] private List<Button> placeButtons;
        [SerializeField] private List<Image> placeImages;
        [SerializeField] private List<GameObject> placeImagesObj;
        [SerializeField] private List<RectTransform> npcPrefrebs;
        [SerializeField] private List<Transform> npcPlaces;
        ExploreStoryController exploreController;

        private List<int> allPlaces = new List<int>();
        private List<int> fixedPlaces = new List<int>();
        private List<int> fixedPlacesImgNum = new List<int>();
        public static List<int> randomPlaces = new List<int>();

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

                int randomNpcNum = UnityEngine.Random.Range(0,2);
                if(randomNpcNum == 1){
                    NpcGenerator(npcPlaces[i],fixedPlaces[i]);
                }
            }
            randomPlaces.AddRange(allPlaces);
            exploreController = GetComponent<ExploreStoryController>();
            for (int i = 0; i < placeButtons.Count; i++)
            {
                int placeIndex = i;
                placeButtons[i].GetComponentInChildren<Text>().text = GetPlaceNameByNum(fixedPlaces[i]);
                placeButtons[i].onClick.AddListener(() => FixedPlaceButtonAct(placeIndex));
            }
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

        public static string GetNpcTypeByNum(int num){
           string npcType = "";
            switch (num)
            {
                case 1:
                    npcType = "monk";
                    break;
                case 2:
                    npcType = "swordsman";
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
            int randomNpcTypeNum = UnityEngine.Random.Range(1,3);
            var newNpc = Instantiate(npcPrefrebs[randomNpcTypeNum-1], npcPlace);

            string npcType = GetNpcTypeByNum(randomNpcTypeNum);
            int randomImgIndex = UnityEngine.Random.Range(1,3);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaNPC/" + npcType + randomImgIndex);

            if (newSprite != null) {
                newNpc.GetChild(0).GetComponentInChildren<Image>().sprite = newSprite;
            }

            newNpc.GetComponent<NpcButtonController>().placeNum = placeNum;
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
