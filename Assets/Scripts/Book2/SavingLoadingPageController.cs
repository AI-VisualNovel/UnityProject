using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SavingLoadingPageController : MonoBehaviour
{

    public string imagePath = "/Art/ScreenShots/";
    public GameObject saveload1_1;
    public GameObject saveload1_2;
    public GameObject saveload1_3;
    public GameObject saveload1_4;
    public GameObject saveload2_1;
    public GameObject saveload2_2;
    public GameObject saveload2_3;
    public GameObject saveload2_4;
    public GameObject saveload3_1;
    public GameObject saveload3_2;
    public GameObject saveload3_3;
    public GameObject saveload3_4;


    
    public static string img; // will receive the newest img name from NavController


    void Start(){
        NametheButtons();
    } 

    public void Synchronize(Button LoadPage_button){
        string fullPath = Application.dataPath + imagePath + img + ".png";

        byte[] imageData = File.ReadAllBytes(fullPath);
        Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D
        texture.LoadImage(imageData); // Load the image data into the Texture2D
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        // Set the sprite as the source image of the button
        Image LoadPage_buttonImage = LoadPage_button.image;
        LoadPage_buttonImage.sprite = sprite;
    }

    public void LoadImage(Button button)
    {
        string fullPath = Application.dataPath + imagePath + img + ".png";
        // string fullPath = Application.dataPath + "/Art/ScreenShots/toothless.jpg";

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

            img = img + "already_saved";
            Debug.Log(button.name);

        }
        else
        {
            Debug.LogError("Image not found at path: " + fullPath);
        }
    }


    public void LoadGame(Button button){
        // img = img - "already_saved";
        // string readFromFilePath = Application.streamingAssetsPath + "/Chat_Logs/" + "Chat " + img + ".txt";
        // List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
        // foreach (string line in fileLines){ 
        //     Debug.Log(line);

        // }
    }


    public void UpdateJsonLocation(GameData data, Button button){
            data.Location = button.name;
    }

    public void NametheButtons(){
        // set buttons name
        saveload1_1.name = "1_1";
        saveload1_2.name = "1_2";
        saveload1_3.name = "1_3";
        saveload1_4.name = "1_4";
        saveload2_1.name = "2_1";
        saveload2_2.name = "2_2";
        saveload2_3.name = "2_3";
        saveload2_4.name = "2_4";
        saveload3_1.name = "3_1";
        saveload3_2.name = "3_2";
        saveload3_3.name = "3_3";
        saveload3_4.name = "3_4";
    }

}
