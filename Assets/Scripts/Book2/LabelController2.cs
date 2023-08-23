using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LabelController2 : MonoBehaviour
{

    // booleans to show which page to go to
    public static bool toHistoryPage;
    public static bool toSavePage;
    public static bool toLoadPage_inGame;
    public static bool toSettingPage_inGame;
    public static bool toAboutPage_inGame;
    public static bool toHelpPage_inGame;

    // which to display
    public GameObject HistoryPage;
    public GameObject SavePage;
    public GameObject LoadPage_inGame;
    public GameObject SettingPage_inGame;
    public GameObject AboutPage_inGame;
    public GameObject HelpPage_inGame;

    // labels
    public Button label1;
    public Button label2;
    public Button label3;
    public Button label4;
    public Button label5;
    public Button label6;
    public Button label7;
    public Button label8;

    void Start()
    {

        HistoryPage.SetActive(false);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);


        if (toHistoryPage == true)
        {
            label1_pressed();
        }
        if (toSavePage == true)
        {
            label2_pressed();
        }
        if (toLoadPage_inGame == true)
        {
            label3_pressed();
        }
        if (toSettingPage_inGame == true)
        {
            label4_pressed();
        }
        if (toAboutPage_inGame == true)
        {
            label5_pressed();
        }
        if (toHelpPage_inGame == true)
        {
            label6_pressed();
        }

    }

    // history page
    public void label1_pressed()
    {
        HistoryPage.SetActive(true);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);
    }

    // save page
    public void label2_pressed()
    {
        HistoryPage.SetActive(false);
        SavePage.SetActive(true);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);
    }

    // load page
    public void label3_pressed()
    {
        HistoryPage.SetActive(false);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(true);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);

        // SavingLoadingPageController.LoadAllImage();
    }
    // setting page
    public void label4_pressed()
    {
        HistoryPage.SetActive(false);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(true);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);
    }
    // about page
    public void label5_pressed()
    {
        HistoryPage.SetActive(false);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(true);
        HelpPage_inGame.SetActive(false);

    }
    // help page
    public void label6_pressed()
    {
        HistoryPage.SetActive(false);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(true);
    }
    // main page
    public void label7_pressed()
    {
        SceneManager.LoadScene("MainPage");
    }
    // quit
    public void label8_pressed()
    {
        Debug.Log("ExitGame!");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
}



