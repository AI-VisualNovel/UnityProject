using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NavController : MonoBehaviour
{

    // public Button SaveButton;
    public LabelController2 Book2;
    public static string newest_screenshot;

    public void Start()
    {
        LabelController2.toHistoryPage = false;
        LabelController2.toSavePage = false;
        LabelController2.toLoadPage_inGame = false;
        LabelController2.toSettingPage_inGame = false;
        LabelController2.toAboutPage_inGame = false;
        LabelController2.toHelpPage_inGame = false;
    }

    public void ScreenShot()
    {
        ScreenCapture.CaptureScreenshot("Assets/Art/ScreenShots/screenshot " + System.DateTime.Now.ToString("MM-dd-yy-HH-mm-ss") + ".png");
        newest_screenshot = "screenshot " + System.DateTime.Now.ToString("MM-dd-yy-HH-mm-ss");
        Debug.Log("A screenshot was taken and saved as " + newest_screenshot + "!");
    }

    public void toSavePAge()
    {
        LabelController2.toSavePage = true;
        ScreenShotsController.img = newest_screenshot;
        SceneManager.LoadScene("Book2");
    }

    public void toHistoryPage()
    {
        LabelController2.toHistoryPage = true;
        SceneManager.LoadScene("Book2");
    }

    public void toSettingPage_inGame()
    {
        LabelController2.toSettingPage_inGame = true;
        SceneManager.LoadScene("Book2");
    }
}

