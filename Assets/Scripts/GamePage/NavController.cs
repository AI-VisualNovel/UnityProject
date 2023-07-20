using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NavController : MonoBehaviour
{

    public Button SaveButton;
    public LabelController2 Book2;
    public static string newest_screenshot;

    public void ScreenShot()
    {
        ScreenCapture.CaptureScreenshot("Assets/Art/ScreenShots/screenshot " + System.DateTime.Now.ToString("MM-dd-yy-HH-mm-ss") + ".png");
        newest_screenshot = "screenshot" + System.DateTime.Now.ToString("MM-dd-yy-HH-mm-ss");
        Debug.Log("A screenshot was taken and saved as " + newest_screenshot + "!");
    }

    public void LoadBook2()
    {
        LabelController2.toSavePage = true;
        SceneManager.LoadScene("Book2");
    }

}

