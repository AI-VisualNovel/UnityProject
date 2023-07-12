using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using UnityEditor;

public class LabelController1 : MonoBehaviour
{
    // load script label_controller2's function to here
    public LabelController2 label_controller2;

    public GameObject LoadPage_main;
    public GameObject SettingPage_main;
    public GameObject AboutPage_main;
    public GameObject HelpPage_main;

    public Button label1;
    public Button label2;
    public Button label3;
    public Button label4;
    public Button label5;
    public Button label6;
    public Button label7;
    public Button label8;


    void start(){
        // label_controller2 = GameObject.FindGameObjectWithTag("LabelController2").GetComponent<LabelController2>();
        label3_pressed();
    }

    // history page
    public void label1_pressed(){ 
        // SceneManager.LoadScene("Book2");
        label_controller2.label1_pressed();
    }

    // save page
    public void label2_pressed(){ 
        // SceneManager.LoadScene("Book2");
        label_controller2.label2_pressed();
    }

    // load page
    public void label3_pressed(){ 
        LoadPage_main.SetActive(true);
        SettingPage_main.SetActive(false);
        AboutPage_main.SetActive(false);
        HelpPage_main.SetActive(false);
    }
    // setting page
    public void label4_pressed(){ 
        SettingPage_main.SetActive(true);
        LoadPage_main.SetActive(false);
        AboutPage_main.SetActive(false);
        HelpPage_main.SetActive(false);
    }
    // about page
    public void label5_pressed(){ 

        AboutPage_main.SetActive(true);
        SettingPage_main.SetActive(false);
        LoadPage_main.SetActive(false);
        HelpPage_main.SetActive(false);
    }
    // help page
    public void label6_pressed(){ 
        HelpPage_main.SetActive(true);
        SettingPage_main.SetActive(false);
        LoadPage_main.SetActive(false);
        AboutPage_main.SetActive(false);
    }
    // main page
    public void label7_pressed(){ 
        SceneManager.LoadScene("MainPage");
    }
    // quit
    public void label8_pressed(){ 
        Debug.Log("ExitGame!");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}
