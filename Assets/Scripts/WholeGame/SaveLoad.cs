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
            // // string json = File.ReadAllText(Application.streamingAssetsPath + "/Json/08-21-23-22-27-51.json");
            // string json = File.ReadAllText(Application.streamingAssetsPath + "/Json/08-22-23-17-03-44.json");
            
            // GameData data = JsonUtility.FromJson<GameData>(json);
            // recallTextObject.GetComponent<Text>().text = data.Story;

            // // string formattedLine = line.Replace(""\n"", "\n");
            // // Instantiate(recallTextObject, contentWindow);
            // // recallTextObject.GetComponent<Text>().text = data.Story;
            // Debug.Log(data.Story);

            string readFromFilePath = Application.streamingAssetsPath + "/Json/08-22-23-17-03-44.json";
            string[] fileLines = File.ReadAllLines(readFromFilePath); // 读取所有行并将其分割成数组

            foreach (string line in fileLines)
            {
                string[] splitLines = line.Split(new string[] { "\n" }, StringSplitOptions.None);

                foreach (string splitLine in splitLines)
                {
                    // 创建新的文本对象并设置其文本内容
                    GameObject newTextObject = Instantiate(recallTextObject, contentWindow);
                    Text textComponent = newTextObject.GetComponent<Text>();
                    textComponent.text = splitLine;
                }
            }



        }
        public void Load(){

            string readFromFilePath = Application.streamingAssetsPath + "/Json/08-22-23-17-03-44.json";
            string[] fileLines = File.ReadAllLines(readFromFilePath); // 读取所有行并将其分割成数组

            foreach (string line in fileLines)
            {
                string[] splitLines = line.Split(new string[] { "<br>" }, StringSplitOptions.None);

                foreach (string splitLine in splitLines)
                {
                    // 创建新的文本对象并设置其文本内容
                    GameObject newTextObject = Instantiate(recallTextObject, contentWindow);
                    Text textComponent = newTextObject.GetComponent<Text>();
                    textComponent.text = splitLine;
                }
            }





            // string readFromFilePath = Application.streamingAssetsPath + "/Json/08-22-23-17-03-44.json";
            // // List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
            // foreach (string line in fileLines){

            //     // 目前還不知道為啥會擠在同一行
            //     // string formattedLine = line.Replace("<br>", "\n");
            //     // Instantiate(recallTextObject, contentWindow);
            //     // recallTextObject.GetComponent<Text>().text = line;
            //     Debug.Log(line);

            // }
        }

        public string ReturnSaveTime(){
            return saved_time;
        }

        public void UpdateJsonLocation(GameData data, Button button){
            data.Location = button.name;
        }

    }
}
