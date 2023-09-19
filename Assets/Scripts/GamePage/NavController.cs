using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static OpenAI.SaveLoad;

public class NavController : MonoBehaviour
{
    public static string newest_screenshot;
    [SerializeField] private GameObject b2panel;
    public bool fromNoApi;

    public void Start()
    {

        b2panel.SetActive(false);

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
        LabelController2.from_game_page = true;

        SceneManager.LoadScene("Book2");
        SavingLoadingPageController.LoadAllImage();

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
    public void setB2Active()
    {
        LabelController2.from_game_setting_btn = true;
        b2panel.SetActive(true);
    }
    public void back(int current)
    {
        if (fromNoApi)
        {
            SceneManager.LoadScene(current);
            fromNoApi = false;

        }
        else
        {
            b2panel.SetActive(false);
        }
    }
    public void NoapiToB2()
    {
        fromNoApi = true;
        LabelController2.from_game_setting_btn = true;
        b2panel.SetActive(true);
    }

}

