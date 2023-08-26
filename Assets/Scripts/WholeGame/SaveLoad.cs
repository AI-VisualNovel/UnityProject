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
    public class SaveLoad : MonoBehaviour
    {
        public OpenAI.NewStoryController new_story_controller;
        public RectTransform contentWindow;
        public GameObject recallTextObject;
        public string saved_time;
        public static string newest_screenshot;
        [SerializeField] private ScrollRect scroll;
        private float height = 0;
        [SerializeField] private RectTransform sent;
        [SerializeField] public RectTransform received; // Lai



        public string Story;
        private List<string> stories_list = new List<string>();

        private List<ChatMessage> Chat = new List<ChatMessage>();
        
        public List<ChatMessage> chat_message;
        


        void Start(){
            // create folder
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Chat_Logs/");
        }

        
        public void ScreenShot(){
            saved_time = System.DateTime.Now.ToString("MM-dd-yy-HH-mm-ss");
            ScreenCapture.CaptureScreenshot("Assets/Art/ScreenShots/" + saved_time + ".png");
            newest_screenshot = saved_time;
            SavingLoadingPageController.saved_time = saved_time;
            SavingLoadingPageController.img = newest_screenshot;
            Debug.Log("A screenshot was taken and saved as " + newest_screenshot + "!");
        }

        public void SaveChatMassage(List<ChatMessage> messages){
            Chat = messages;
        }

        public void SaveStoryToList(string story){
            stories_list.Add(story);
            // Debug.Log(stories_list);
            foreach (string s in stories_list){
                Debug.Log(s);
            }
        }

        // fires after the save buttin been pressed
        public void SaveToJson(){
            GameData data = new GameData();
            data.Time = saved_time;

            string list_to_json = JsonConvert.SerializeObject(Chat);
            data.ChatMessage = list_to_json;

            Debug.Log("saved_time in SaveToJson: " + saved_time);

            foreach (string s in stories_list){
                data.Story = data.Story + "\n" + s;
            }
            data.Location = ""; // will update in SavingLoadingPageController
            string json = JsonUtility.ToJson(data,true);
            File.WriteAllText(Application.streamingAssetsPath + "/Json/" + data.Time + ".json", json);
        }

        public void LoadFromJson(){

            string readFromFilePath = Application.streamingAssetsPath + "/Json/08-25-23-21-59-19.json";
            string[] fileLines = File.ReadAllLines(readFromFilePath); // 读取所有行并将其分割成数组

            List<ChatMessage> chat_massage = GetChatMassage(readFromFilePath);
            foreach (ChatMessage m in chat_massage){
                Debug.Log(m.Role);
                Debug.Log(m.Content);
                var recItem = AppendMessage(m);


                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                recItem.anchoredPosition = new Vector2(0, -height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(recItem);
                height += recItem.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;
            }

        }

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
        


        public static List<ChatMessage> GetChatMassage(string filepath){
            string json = File.ReadAllText(filepath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);
            List<ChatMessage> chatMessages = JsonConvert.DeserializeObject<List<ChatMessage>>(data.ChatMessage);
            return chatMessages;
        }
        

        public string ReturnSaveTime(){
            return saved_time;
        }

        public void UpdateJsonLocation(GameData data, Button button){
            data.Location = button.name;
        }

    }
}
