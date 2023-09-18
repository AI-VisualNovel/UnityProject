using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;
using System;


public class SavingLoadingPageController : MonoBehaviour
{

    public static string imagePath = "/Art/ScreenShots/";
    public Button save1_1;
    public Button save1_2;
    public Button save1_3;
    public Button save1_4;

    // 遊戲進度的名字
    public GameObject name1_1;
    public GameObject name1_2;
    public GameObject name1_3;
    public GameObject name1_4;


    // SavingMessagePanel的東東
    public GameObject SavingMessagePanel;
    public Button Cancel;
    public Button Enter;
    public GameObject returnButton;



    public static string img; // will receive the newest img name from SaveLoad
    public static string saved_time;

    public static bool from_game_page = false;
    public static bool toSavePage = false;
    public static bool toLoadPage = true;



    public Button btn_been_pressed; // 存是哪一個save btn被點到



    void Start()
    {

        SavingMessagePanel.SetActive(false);
        NameGameObjects();
        if(from_game_page == true){
            LoadAllImage();
            returnButton.SetActive(false); // 存完才可以按返回
        }
    }

    // 判斷當前button有沒有存其他進度
    public void CheckAvailable(Button button){
        btn_been_pressed = button; // 傳當前點到的btn過去
        GameObject foundGameObject = GameObject.Find("name" + button.name); 
        string text = foundGameObject.GetComponent<Text>().text;
        if(text != ""){ // 有存東西
            SavingMessagePanel.SetActive(true);
        }else{
            LoadImage(button);
            // book2.to load 
            LabelController2.new_game_been_saved = true;
            returnButton.SetActive(true); // 存完才可以按返回
        }
    }

    public void CancelOverwrite()
    {
        SavingMessagePanel.SetActive(false);
    }


    public void ConfirmOverwrite()
    {
        SavingMessagePanel.SetActive(false);
        LoadImage(btn_been_pressed); 
        // book2.to load page
        LabelController2.new_game_been_saved = true;

    }


    public void LoadImage(Button button)
    {
        string fullPath = Application.dataPath + imagePath + img + ".png";

        if (img == "already_saved")
        {
            Debug.LogError("You have already saved your game progress!");
        }
        else if (File.Exists(fullPath))
        {
            byte[] imageData = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D
            texture.LoadImage(imageData); // Load the image data into the Texture2D
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            // Set the sprite as the source image of the button
            Image buttonImage = button.image;
            buttonImage.sprite = sprite;

            // // 同步顯示名字
            LoadName(img,button.name);

            // 把位置也包到Json
            UpdateJsonLocation(button);
            img = img + "already_saved";
        }
    }




    // 載入所有截圖
    public static void LoadAllImage()
    {
        // 获取 JSON 文件列表
        string jsonFolderPath = Application.streamingAssetsPath + "/Json";
        string[] jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");

        // 遍历 JSON 文件
        foreach (string jsonFilePath in jsonFiles)
        {
            string json = File.ReadAllText(jsonFilePath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);

            
            // 根据 ImageLocation 加载图像到对应按钮
            // 通过按钮名称查找按钮

            

            GameObject foundObject = GameObject.Find(data.Location);
            if (foundObject != null)
            {
                Button foundButton = foundObject.GetComponent<Button>();
                if (foundButton != null)
                {
                    LoadName(data);

                    string fullPath = Application.dataPath + imagePath + data.Time + ".png";
                    byte[] imageData = File.ReadAllBytes(fullPath);
                    Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D
                    texture.LoadImage(imageData); // Load the image data into the Texture2D
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                    // Set the sprite as the source image of the button
                    Image buttonImage = foundButton.image;
                    buttonImage.sprite = sprite;
                }else
                {
                    Debug.LogError("找到的GameComponent不包含 Button：" + data.Location);
                }
            }else
            {
                Debug.LogError("找不到GameComponent：" + data.Location);
            }
        }
    }

    // 載入遊戲進度的名字
    public static void LoadName(GameData data){


        string jsonfile_name = data.Time;
        // 找存name的GameObject
        GameObject foundGameObject = GameObject.Find("name" + data.Location);
        string toprint = "";
        string date = "";

        date = jsonfile_name[1].ToString() + jsonfile_name[2] + "月" + jsonfile_name[3] + jsonfile_name[4] + "日";
      
        if (jsonfile_name[0] == '1')
        {
            toprint = "武俠" + date;
        }
        else if (jsonfile_name[0] == '2')
        {
            toprint = "靈異" + date;
        }
        else
        {
            toprint = "奇幻" + date;
        }

        foundGameObject.GetComponent<Text>().text = toprint;
    }

    public static void LoadName(string jsonfile_name, string button_name){

        // 找存name的GameObject
        GameObject foundGameObject = GameObject.Find("name" + button_name);
        string toprint = "";
        string date = "";

        date = jsonfile_name[1].ToString() + jsonfile_name[2] + "月" + jsonfile_name[3] + jsonfile_name[4] + "日";
      
        if (jsonfile_name[0] == '1')
        {
            toprint = "武俠" + date;
        }
        else if (jsonfile_name[0] == '2')
        {
            toprint = "靈異" + date;
        }
        else
        {
            toprint = "奇幻" + date;
        }

        foundGameObject.GetComponent<Text>().text = toprint;
    }


    public void LoadGame(string jsonfile_name)
    { 
        
        // 跳轉化面並載入遊戲
        OpenAI.NewStoryController.from_book2 = true;
        OpenAI.SaveLoad.from_book2 = true;
        OpenAI.SaveLoad.jsonfile_name = jsonfile_name;
        // SceneManager.LoadScene("GamePage");

        // 測試Legacy用
        OpenAI.SaveLoadLegacy.from_book2 = true;
        OpenAI.SaveLoadLegacy.jsonfile_name = jsonfile_name;
        if (jsonfile_name[0] == '1')
        {
            OpenAI.WuxiaStoryController.from_book2 = true;
            SceneManager.LoadScene("Legacy_WuXia");
        }
        if (jsonfile_name[0] == '2')
        {
            OpenAI.GhostStoryController.from_book2 = true;
            SceneManager.LoadScene("Legacy_Ghost");
        }
        if (jsonfile_name[0] == '3')
        {
            OpenAI.FantasyStoryController.from_book2 = true;
            SceneManager.LoadScene("Legacy_Fantasy");
        }

    }



    public void LoadGame(Button button)
    { 
        
        string jsonfile_name = GetJsonfile(button);
        
        // 跳轉化面並載入遊戲
        OpenAI.NewStoryController.from_book2 = true;
        OpenAI.SaveLoad.from_book2 = true;
        OpenAI.SaveLoad.jsonfile_name = jsonfile_name;
        // SceneManager.LoadScene("GamePage");

        // 測試Legacy用
        OpenAI.SaveLoadLegacy.from_book2 = true;
        OpenAI.SaveLoadLegacy.jsonfile_name = jsonfile_name;
        if (jsonfile_name[0] == '1')
        {
            OpenAI.WuxiaStoryController.from_book2 = true;
            SceneManager.LoadScene("Legacy_WuXia");
        }
        if (jsonfile_name[0] == '2')
        {
            OpenAI.GhostStoryController.from_book2 = true;
            SceneManager.LoadScene("Legacy_Ghost");
        }
        if (jsonfile_name[0] == '3')
        {
            OpenAI.FantasyStoryController.from_book2 = true;
            SceneManager.LoadScene("Legacy_Fantasy");
        }

    }

    // 從按鈕去找json file
    public string GetJsonfile(Button button)
    {

        string json_name = "Empty";
        string jsonFolderPath = Application.streamingAssetsPath + "/Json";
        string[] jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");
        foreach (string jsonFilePath in jsonFiles)
        {

            string json = File.ReadAllText(jsonFilePath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);

            if (data.Location == button.name)
            {
                json_name = data.Time;
            }
        }
        // Debug.Log(json_name);
        return json_name;  // return json file name inorder to load the correct json file
    }


    public void UpdateJsonLocation(Button button)
    {

        string json = File.ReadAllText(Application.streamingAssetsPath + "/Json/" + saved_time + ".json");

        // 將JSON解析為C#對象
        GameData data = JsonConvert.DeserializeObject<GameData>(json);
        data.Location = button.name;
        json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.streamingAssetsPath + "/Json/" + saved_time + ".json", json);

        string json2 = File.ReadAllText(Application.streamingAssetsPath + "/Json/" + saved_time + ".json");
        GameData data2 = JsonUtility.FromJson<GameData>(json2);

    }

    public void NameGameObjects()
    {

        
        // set buttons name
        save1_1.gameObject.name = "1_1";
        save1_2.gameObject.name = "1_2";
        save1_3.gameObject.name = "1_3";
        save1_4.gameObject.name = "1_4";

        // load1_1.gameObject.name = "load1_1";
        // load1_2.gameObject.name = "load1_2";
        // load1_3.gameObject.name = "load1_3";
        // load1_4.gameObject.name = "load1_4";

        // set gameobject text name
        name1_1.name = "name1_1";
        name1_2.name = "name1_2";
        name1_3.name = "name1_3";
        name1_4.name = "name1_4";

    }
    public string GetLatestJsonFile()
    {
        string jsonFolderPath = Application.streamingAssetsPath + "/Json";

        string[] jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");

        if (jsonFiles.Length == 0)
            return null;

        string latestFileFullPath = jsonFiles.OrderByDescending(file =>
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(file);

            string numberPart = filenameWithoutExtension.Substring(1);

            if (Int64.TryParse(numberPart, out long result))
            {
                return result;
            }
            return 0;
        }).FirstOrDefault();

        string latestFileNameWithoutExtension = Path.GetFileNameWithoutExtension(latestFileFullPath);
        Debug.Log(latestFileNameWithoutExtension);
        return latestFileNameWithoutExtension;
    }



    public void LoadLatestGame()
    {
        string jsonfile_name = GetLatestJsonFile();
        LoadGame(jsonfile_name);
    }
}
