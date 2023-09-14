using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class NewLoadPageController : MonoBehaviour
{

    public static string imagePath = "/Art/ScreenShots/";

    public Button load1_1;
    public Button load1_2;
    public Button load1_3;
    public Button load1_4;

    // 遊戲進度的名字
    public GameObject name1_1;
    public GameObject name1_2;
    public GameObject name1_3;
    public GameObject name1_4;


    // Start is called before the first frame update
    void Start()
    {
        NameGameObjects();
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


    public void NameGameObjects()
    {

        // set buttons name
        load1_1.gameObject.name = "1_1";
        load1_2.gameObject.name = "1_2";
        load1_3.gameObject.name = "1_3";
        load1_4.gameObject.name = "1_4";

        // set gameobject text name
        name1_1.name = "name1_1";
        name1_2.name = "name1_2";
        name1_3.name = "name1_3";
        name1_4.name = "name1_4";

    }
}
