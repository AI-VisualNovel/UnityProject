using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenShotsController : MonoBehaviour
{


    public GameObject saveload1;
    public GameObject saveload2;
    public GameObject saveload3;
    public GameObject saveload4;

    public string imagePath = "/Art/ScreenShots/";
    public static string img; // will receive the newest img name from NavController


    public void LoadImage(Button button)
    {
        string fullPath = Application.dataPath + imagePath + img + ".png";
        // string fullPath = Application.dataPath + "/Art/ScreenShots/toothless.jpg";


        if (File.Exists(fullPath))
        {
            byte[] imageData = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D
            texture.LoadImage(imageData); // Load the image data into the Texture2D
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            // Set the sprite as the source image of the button
            Image buttonImage = button.image;
            buttonImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Image not found at path: " + fullPath);
        }
    }



}
