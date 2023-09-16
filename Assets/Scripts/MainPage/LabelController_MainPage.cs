using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LabelController_MainPage : MonoBehaviour
{
    public FadeText fadeScript;

    // booleans to show which page to go to
    
    public static bool toLoadPage = false;
    public static bool toSettingPage = false;
    // public static bool toAboutPage;
    // public static bool toHelpPage;

    // which to display
    public GameObject LoadPage;
    public GameObject SettingPage;
    public GameObject AboutPage;
    public GameObject HelpPage;

    public GameObject Book2;


    // labels
    public Button label3;
    public GameObject label3_native_size;
    public Button label4;
    public GameObject label4_native_size;
    public Button label5;
    public Button label6;
    public Button label7;
    public Button label8;


    void Start()
    {

        LoadPage.SetActive(false);
        SettingPage.SetActive(false);
        AboutPage.SetActive(false);
        HelpPage.SetActive(false);

        
        label3_native_size.SetActive(false);
        label4_native_size.SetActive(false);

        if (toLoadPage == true)
        {
            label3_native_size.SetActive(true);
            label3_pressed();
        }
        if (toSettingPage == true)
        {
            label4_native_size.SetActive(true);
            label4_pressed();
        }
    }

    // load page
    public void label3_pressed()
    {
        // labels
        label4_native_size.SetActive(false);

        LoadPage.SetActive(true);
        SettingPage.SetActive(false);
        AboutPage.SetActive(false);
        HelpPage.SetActive(false);

        SavingLoadingPageController.LoadAllImage();
        toSettingPage = false;

    }
    // setting page
    public void label4_pressed()
    {
        // labels
        label3_native_size.SetActive(false);
        
        LoadPage.SetActive(false);
        SettingPage.SetActive(true);
        AboutPage.SetActive(false);
        HelpPage.SetActive(false);

        toLoadPage = false;
    }

    // about page
    public void label5_pressed()
    {
        // labels
        label4_native_size.SetActive(false);

        LoadPage.SetActive(false);
        SettingPage.SetActive(false);
        AboutPage.SetActive(true);
        HelpPage.SetActive(false);

        fadeScript.FadeIn();
        

    }
    // help page
    public void label6_pressed()
    {
        // labels
        label4_native_size.SetActive(false);

        LoadPage.SetActive(false);
        SettingPage.SetActive(false);
        AboutPage.SetActive(false);
        HelpPage.SetActive(true);
    }
    // main page
    public void label7_pressed()
    {
        Book2.SetActive(false);
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

    public void HideBook2(){
        Book2.SetActive(false);
    }
    
}



