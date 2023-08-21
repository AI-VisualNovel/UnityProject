using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEditor;
using System.IO;
using System.Linq;

namespace OpenAI
{
    public class SaveLoad : MonoBehaviour
    {
        public OpenAI.NewStoryController new_story_controller;
        // public RectTransform contentWindow;
        public Transform contentWindow;
        public GameObject recallTextObject;
        public static string saved_time;

        // public GameObject scroll;
        // public GameObject userMessagePrefab;
        // public Transform messageContainer;
        // private OpenAIApi openai = new OpenAIApi("sk-DIaCIeZ4lJQKAOCPXi8gT3BlbkFJPIUASXIefhkBjbQy4");

        public string Story;
        private List<string> stories_list = new List<string>();
        



        void Start(){
            // create folder
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Chat_Logs/");
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
            data.Time = System.DateTime.Now.ToString("MM-dd-yy-HH-mm-ss");
            saved_time = data.Time;
            foreach (string s in stories_list){
                data.Story = data.Story + "\n" + s;
            }
            data.Location = ""; // will update in SavingLoadingPageController
            string json = JsonUtility.ToJson(data,true);
            File.WriteAllText(Application.streamingAssetsPath + "/Json/" + data.Time + ".json", json);
        }

        public void LoadFromJson(){
            // string json = File.ReadAllText(Application.streamingAssetsPath + "/Json/08-21-23-22-27-51.json");
            string json = File.ReadAllText(Application.streamingAssetsPath + "/Json/08-21-23-22-56-52.json");
            
            GameData data = JsonUtility.FromJson<GameData>(json);
            recallTextObject.GetComponent<Text>().text = data.Story;

            // string formattedLine = line.Replace(""\n"", "\n");
            // Instantiate(recallTextObject, contentWindow);
            // recallTextObject.GetComponent<Text>().text = data.Story;
            Debug.Log(data.Story);

        }



        public void CreateTextFile(string story){
            string txtDocumentName = Application.streamingAssetsPath + "/Chat_Logs/" + "Chat" + ".txt";

            if (!File.Exists(txtDocumentName)){
                File.WriteAllText(txtDocumentName, "Here's the Story\n");
            }

            File.AppendAllText(txtDocumentName, story + "\n");

        }
        
        // Start is called before the first frame update
        public void Save(){
        
            // rename the file as same as screenshots
            string oldName = Application.streamingAssetsPath + "/Chat_Logs/" + "Chat" + ".txt";
            string newName = Application.streamingAssetsPath + "/Chat_Logs/" + "Chat " + System.DateTime.Now.ToString("MM-dd-yy-HH-mm-ss") + ".txt";
            System.IO.File.Move(oldName, newName);
        }

        public void Load(){
            string readFromFilePath = Application.streamingAssetsPath + "/Chat_Logs/" + "Chat" + ".txt";
            List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
            foreach (string line in fileLines){

                // 目前還不知道為啥會擠在同一行
                // string formattedLine = line.Replace("<br>", "\n");
                // Instantiate(recallTextObject, contentWindow);
                // recallTextObject.GetComponent<Text>().text = line;
                Debug.Log(line);

            }
        }


    }
}
