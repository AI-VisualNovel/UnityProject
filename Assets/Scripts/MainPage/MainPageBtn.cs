using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class MainPageBtn : MonoBehaviour
{
    // [SerializeField] private SavingLoadingPageController slpc;
    public AudioSource soundPlayer;
    [SerializeField] private GameObject B2;

    [SerializeField] private Button ViewLoadGameButton;

    [SerializeField] private Button SettingButton;

    void Start()
    {
        Debug.Log("Start!");
    }

    void Update()
    {

    }
    public void PrintNewGameMessage()
    {
        Debug.Log("NewGame!");
        SceneManager.LoadScene("GamSelect");
        soundPlayer.Play();
    }


    // 繼續遊戲
    public void PrintContinueGameMessage()
    {
        PlayerPrefs.SetString("ContinueGame", "1");
        PlayerPrefs.Save();

        Debug.Log("ContinueGame!");
        soundPlayer.Play();
        NewLoadPageController.LoadLatestGame();
    }

    // 讀取暫存
    public void PrintViewLoadingMessage()
    {
        Debug.Log("ViewLoading!");
        B2.SetActive(true);
        LabelController_MainPage.toLoadPage = true;

        soundPlayer.Play();
    }


    public void PrintSettingMessage()
    {
        Debug.Log("Setting!");
        LabelController_MainPage.toSettingPage = true;

        B2.SetActive(true);
        soundPlayer.Play();
    }
    public void PrintExitGameMessage()
    {
        Debug.Log("ExitGame!");
        soundPlayer.Play();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
