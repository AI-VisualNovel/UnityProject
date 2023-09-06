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
    public class FullStoryControllerFantasy : MonoBehaviour
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
        ExploreStoryControllerFantasy exploreController;

        private List<int> allPlaces = new List<int>();
        private List<int> fixedPlaces = new List<int>();
        private List<int> fixedPlacesImgNum = new List<int>();
        public static List<int> randomPlaces = new List<int>();

        private void Start()
        {
            //分配地圖
            for (int i = 1; i <= 16; i++){
                allPlaces.Add(i);
            }
            for (int i = 0; i < 10; i++){
                int randomIndex = UnityEngine.Random.Range(0, allPlaces.Count);
                int randomNumber = allPlaces[randomIndex];
                fixedPlaces.Add(randomNumber);
                allPlaces.RemoveAt(randomIndex);

                int randomImgIndex = UnityEngine.Random.Range(1,5);
                Sprite newSprite = Resources.Load<Sprite>("FantasyBackground/" + fixedPlaces[i] + "/" + randomImgIndex);
                placeImages[i].sprite = newSprite;

                int randomNpcNum = UnityEngine.Random.Range(0,2);
                if(randomNpcNum == 1){
                    NpcGenerator(npcPlaces[i]);
                }
            }
            randomPlaces.AddRange(allPlaces);
            exploreController = GetComponent<ExploreStoryControllerFantasy>();
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
                    placeName = "城堡";
                    break;
                case 2:
                    placeName = "城市";
                    break;
                case 3:
                    placeName = "塔樓";
                    break;
                case 4:
                    placeName = "山林";
                    break;
                case 5:
                    placeName = "巢穴";
                    break;
                case 6:
                    placeName = "銀河";
                    break;
                case 7:
                    placeName = "星塵";
                    break;
                case 8:
                    placeName = "森林";
                    break;
                case 9:
                    placeName = "沙漠";
                    break;
                case 10:
                    placeName = "洞穴";
                    break;
                case 11:
                    placeName = "湖泊";
                    break;
                case 12:
                    placeName = "瀑布";
                    break;
                case 13:
                    placeName = "秘境";
                    break;
                case 14:
                    placeName = "花園";
                    break;
                case 15:
                    placeName = "星雲";
                    break;
                case 16:
                    placeName = "魔法學院";
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

        private void NpcGenerator(Transform npcPlace){
            int randomNpcTypeNum = UnityEngine.Random.Range(1,3);
            var newNpc = Instantiate(npcPrefrebs[randomNpcTypeNum-1], npcPlace);

            string npcType = GetNpcTypeByNum(randomNpcTypeNum);
            int randomImgIndex = UnityEngine.Random.Range(1,3);
            Sprite newSprite = Resources.Load<Sprite>("WuxiaNPC/" + npcType + randomImgIndex);

            if (newSprite != null) {
                newNpc.GetChild(0).GetComponentInChildren<Image>().sprite = newSprite;
            }
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
