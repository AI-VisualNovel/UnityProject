using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LabelController2 : MonoBehaviour
{
    public FadeText fadeScript;

    // booleans to show which page to go to
    public static bool toHistoryPage;
    public static bool toSavePage;
    public static bool toLoadPage_inGame;
    public static bool toSettingPage_inGame;
    public static bool toAboutPage_inGame;
    public static bool toHelpPage_inGame;


    // privious page
    public static bool from_main_page = false;
    public static bool from_game_page = false; // 只有要存檔的時候才會從gamepage來
    public static bool from_game_setting_btn = false;
    public static bool new_game_been_saved = false;
   



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
    public GameObject label2_native_size;
    public Button label3;
    public GameObject label3_native_size;
    public Button label4;
    public GameObject label4_native_size;
    public Button label5;
    public Button label6;
    public Button label7;
    public Button label8;

    public GameObject returnButton;




    // public Animator buttonAnimator; // 你的按钮上的Animator组件


    void Start()
    {

        HistoryPage.SetActive(false);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(true); // 初始畫面在設定頁面
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);

        label2_native_size.SetActive(false);
        label3_native_size.SetActive(false);
        label4_native_size.SetActive(true);

        // 如果從main page過來，不能進入history page & save page
        if(from_main_page == true){
            label1.gameObject.SetActive(false);
            label2.gameObject.SetActive(false);
            label2_native_size.SetActive(false);
        }

        if(from_game_page == true){ // 要存檔
            label1.gameObject.SetActive(false);
            // label2.SetActive(false);
            // label2_native_size.SetActive(false);
            label3.gameObject.SetActive(false);
            label3_native_size.gameObject.SetActive(false);
            label4.gameObject.SetActive(false);
            label4_native_size.gameObject.SetActive(false);
            label5.gameObject.SetActive(false);
            label6.gameObject.SetActive(false);
            label7.gameObject.SetActive(false);
            label8.gameObject.SetActive(false);

            returnButton.SetActive(false);

        }

        if (toHistoryPage == true)
        {
            label1_pressed();
        }
        if (toSavePage == true) // 只有從game page按存檔才會過來
        {
            SavingLoadingPageController.LoadAllImage();
            label2_native_size.SetActive(true);
            label2_pressed();
        }
        if (toLoadPage_inGame == true)
        {
            label3_native_size.SetActive(true);
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
    private void Update()
    {
        // 如果從game page的setting button過來，不能進入load page & save page
        if(from_game_setting_btn == true){
            label2.gameObject.SetActive(false);
            label2_native_size.SetActive(false);
            label3.gameObject.SetActive(false);
            label3_native_size.SetActive(false);
        }

        // 有新的存檔存完就會跳轉到讀檔頁面
        if(new_game_been_saved == true){
            label3_native_size.SetActive(true);
            label3_pressed();

            // 就不能再回去存檔畫面了
            label2.gameObject.SetActive(false);
            label2_native_size.SetActive(false);
            new_game_been_saved = false; // reset
        }
    }

    // history page
    public void label1_pressed()
    {
        // labels
        label2_native_size.SetActive(false);
        label4_native_size.SetActive(false);


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
        label2_native_size.SetActive(true);
        label4_native_size.SetActive(false);

        HistoryPage.SetActive(false);
        SavePage.SetActive(true);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);

        SavingLoadingPageController.LoadAllImage();

    }

    // load page
    public void label3_pressed()
    {
        // labels
        label2_native_size.SetActive(false);
        label4_native_size.SetActive(false);


        HistoryPage.SetActive(false);
        SavePage.SetActive(false); // 共用畫面了
        LoadPage_inGame.SetActive(true);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(false);
        HelpPage_inGame.SetActive(false);

        SavingLoadingPageController.LoadAllImage();

    }
    // setting page
    public void label4_pressed()
    {
        // labels
        label2_native_size.SetActive(false);
        label4_native_size.SetActive(true);
        
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
        // labels
        label2_native_size.SetActive(false);
        label4_native_size.SetActive(false);

        
        HistoryPage.SetActive(false);
        SavePage.SetActive(false);
        LoadPage_inGame.SetActive(false);
        SettingPage_inGame.SetActive(false);
        AboutPage_inGame.SetActive(true);
        HelpPage_inGame.SetActive(false);

        fadeScript.FadeIn();
        

    }
    // help page
    public void label6_pressed()
    {
        // labels
        label2_native_size.SetActive(false);
        label4_native_size.SetActive(false);

        
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



