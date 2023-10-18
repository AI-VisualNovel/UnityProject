using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using Newtonsoft.Json;



namespace OpenAI
{
    public class SaveLoadLegacy : MonoBehaviour
    {

        // public OpenAI.WuxiaStoryController wuxia_story_controller;
        public RectTransform contentWindow;
        public GameObject recallTextObject;
        public string saved_time;
        public static string newest_screenshot;


        // [SerializeField] private static ScrollRect scroll;
        [SerializeField] private ScrollRect scroll;
        private float height = 0;
        [SerializeField] private RectTransform sent;
        [SerializeField] public RectTransform received; // Lai
        [SerializeField] private Text textArea;  // 顯示劇情的地方
        private RectTransform currentMessageRec;

        // testing    
        private int textBoxCount = 0;



        public string Story;
        private List<string> stories_list = new List<string>();

        private List<ChatMessage> Chat = new List<ChatMessage>();

        public List<ChatMessage> chat_message;

        public static bool from_book2 = false;
        public static string jsonfile_name;

        // option
        public Button option1;
        public Button option2;
        public Button option3;
        // image
        public WuxiaStoryController WuxiaStoryController;
        public GhostStoryController GhostStoryController;
        public FantasyStoryController FantasyStoryController;


        public string persistentDataPath;
        void Awake()
        {
            persistentDataPath = Application.persistentDataPath;
        }

        void Start()
        {
            if (from_book2 == true)
            {
                LoadFromJson(jsonfile_name);
            }
        }


        public void ScreenShot(string category)
        {
            saved_time = category + System.DateTime.Now.ToString("MMddyyHHmmss");
            // ScreenCapture.CaptureScreenshot("Assets/Art/ScreenShots/" + saved_time + ".png");
            // ScreenCapture.CaptureScreenshot("Assets/StreamingAssets/ScreenShots/" + saved_time + ".png"); // 10/18測試
            ScreenCapture.CaptureScreenshot(persistentDataPath + "/ScreenShots/" + saved_time + ".png"); // 10/18測試
            newest_screenshot = saved_time;
            SavingLoadingPageController.saved_time = saved_time;
            SavingLoadingPageController.img = newest_screenshot;
            Debug.Log("A screenshot was taken and saved as " + newest_screenshot + "!");
        }

        public void SaveChatMassage(List<ChatMessage> messages)
        {
            Chat = messages;
        }

        public void SaveStoryToList(string story)
        {
            stories_list.Add(story);
            // Debug.Log(stories_list);
            foreach (string s in stories_list)
            {
                Debug.Log(s);
            }
        }

        // fires after the save buttin been pressed
        public void SaveToJson(int storytype)
        {
            SavingLoadingPageController.from_game_page = true; // 從gamepage過去存檔頁面

            GameData data = new GameData();
            data.Time = saved_time;

            string list_to_json = JsonConvert.SerializeObject(Chat);
            data.ChatMessage = list_to_json;

            // 存當前按鈕的文字
            // 獲取最新的按鈕文本內容
            string latestOption1Text = option1.GetComponentInChildren<Text>().text;
            string latestOption2Text = option2.GetComponentInChildren<Text>().text;
            string latestOption3Text = option3.GetComponentInChildren<Text>().text;

            // 將最新的文本內容儲存在檔案
            data.LatestOption1 = latestOption1Text;
            data.LatestOption2 = latestOption2Text;
            data.LatestOption3 = latestOption3Text;


            // 讀取文件路徑

            string imagePath = "";
            if(storytype == 1){
                imagePath = WuxiaStoryController.BackgroundImagePath;
            }
            if(storytype == 2){
                imagePath = GhostStoryController.BackgroundImagePath;
            }
            if(storytype == 3){
                imagePath = FantasyStoryController.BackgroundImagePath;
            }
            
            data.BackgroundImg = imagePath;

            Debug.Log("saved_time in SaveToJson: " + saved_time);

            foreach (string s in stories_list)
            {
                data.Story = data.Story + "\n" + s;
            }
            data.Location = ""; // will update in SavingLoadingPageController
            string json = JsonUtility.ToJson(data, true);
            // File.WriteAllText(Application.streamingAssetsPath + "/Json/" + data.Time + ".json", json);
            File.WriteAllText(persistentDataPath + "/Json/" + data.Time + ".json", json);

        }

        // public static void LoadFromJson(string jsonfile_name){
        public void LoadFromJson(string jsonfile_name)
        {


            // string readFromFilePath = Application.streamingAssetsPath + "/Json/" + jsonfile_name + ".json";
            string readFromFilePath = persistentDataPath + "/Json/" + jsonfile_name + ".json";

            WuxiaStoryController.JsonFilePath = readFromFilePath; // 更新路徑去WuxiaStoryController
            GhostStoryController.JsonFilePath = readFromFilePath;
            FantasyStoryController.JsonFilePath = readFromFilePath;


            string[] fileLines = File.ReadAllLines(readFromFilePath); // 读取所有行并将其分割成数组

            List<ChatMessage> chat_massage = GetChatMassage(readFromFilePath);


            ChatMessage lastMessage;
            foreach (ChatMessage m in chat_massage)
            {
                Debug.Log(m.Role);
                Debug.Log(m.Content);
                // var recItem = AppendMessage(m);

                var sentItem = AppendMessage(m);
                currentMessageRec = sentItem;
                textArea.text = currentMessageRec.GetChild(0).GetChild(0).GetComponent<Text>().text; // 這裡!!!


                // testing
                WuxiaStoryController.from_book2 = true;

                lastMessage = m;
            }
        }

        // private static RectTransform AppendMessage(ChatMessage message)
        private RectTransform AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;

            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;

            return item;
        }

        public List<ChatMessage> GetChatMassage(string filepath)
        {

            string json = File.ReadAllText(filepath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);
            List<ChatMessage> chatMessages = JsonConvert.DeserializeObject<List<ChatMessage>>(data.ChatMessage);
            return chatMessages;
        }


        public string ReturnSaveTime()
        {
            return saved_time;
        }

        public void UpdateJsonLocation(GameData data, Button button)
        {
            data.Location = button.name;
        }

        public static GameData GetGameData(string filepath)
        {
            string json = File.ReadAllText(filepath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);
            return data;
        }



    }
}
