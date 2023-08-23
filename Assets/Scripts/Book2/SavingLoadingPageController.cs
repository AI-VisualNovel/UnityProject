using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
// using System;
// using System.Windows.Forms;

public class SavingLoadingPageController : MonoBehaviour
{

    public string imagePath = "/Art/ScreenShots/";
    public Button saveload1_1;
    public Button saveload1_2;
    public Button saveload1_3;
    public Button saveload1_4;
    public Button saveload2_1;
    public Button saveload2_2;
    public Button saveload2_3;
    public Button saveload2_4;
    public Button saveload3_1;
    public Button saveload3_2;
    public Button saveload3_3;
    public Button saveload3_4;


    
    public static string img; // will receive the newest img name from NavController
    public static string saved_time;


    void Start(){
        NametheButtons();
        saveload1_1.gameObject.SetActive(true);
    } 

    public void LoadImage(Button button)
    {
        string fullPath = Application.dataPath + imagePath + img + ".png";

        if(img == "already_saved"){
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


            // 把位置也包到Json
            UpdateJsonLocation(button);
            img = img + "already_saved";
        }
        else
        {
            Debug.LogError("Image not found at path: " + fullPath);
        }
    }

    public void LoadAllImage(){
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
            Button foundButton = GameObject.Find(data.Location).GetComponent<Button>();
           
            
            string fullPath = Application.dataPath + imagePath + data.Location + ".png";
            byte[] imageData = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D
            texture.LoadImage(imageData); // Load the image data into the Texture2D
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            // Set the sprite as the source image of the button
            Image buttonImage = foundButton.image;
            buttonImage.sprite = sprite;
            
        }
    }
    


    // public void LoadGame(Button button){
    //     // img = img - "already_saved";
    //     // string readFromFilePath = Application.streamingAssetsPath + "/Chat_Logs/" + "Chat " + img + ".txt";
    //     // List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
    //     // foreach (string line in fileLines){ 
    //     //     Debug.Log(line);

    //     // }
    // }


    public void UpdateJsonLocation(Button button){
       
        string json = File.ReadAllText(Application.streamingAssetsPath + "/Json/" + saved_time + ".json");

        // 將JSON解析為C#對象
        GameData data = JsonConvert.DeserializeObject<GameData>(json);
        data.Location = button.name;
        json = JsonUtility.ToJson(data,true);
        File.WriteAllText(Application.streamingAssetsPath + "/Json/" + saved_time + ".json", json);

        string json2 = File.ReadAllText(Application.streamingAssetsPath + "/Json/" + saved_time + ".json");
        GameData data2 = JsonUtility.FromJson<GameData>(json2);
        
    }

    public void NametheButtons(){
        // set buttons name
        saveload1_1.gameObject.name = "1_1";
        saveload1_2.gameObject.name = "1_2";
        saveload1_3.gameObject.name = "1_3";
        saveload1_4.gameObject.name = "1_4";
        saveload2_1.gameObject.name = "2_1";
        saveload2_2.gameObject.name = "2_2";
        saveload2_3.gameObject.name = "2_3";
        saveload2_4.gameObject.name = "2_4";
        saveload3_1.gameObject.name = "3_1";
        saveload3_2.gameObject.name = "3_2";
        saveload3_3.gameObject.name = "3_3";
        saveload3_4.gameObject.name = "3_4";
    }

}
