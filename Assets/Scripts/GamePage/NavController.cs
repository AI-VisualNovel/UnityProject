using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static OpenAI.SaveLoad;

public class NavController : MonoBehaviour
{
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

    public void toSavePAge()
    {
        LabelController2.toSavePage = true;
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

