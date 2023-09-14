using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainPageBtn : MonoBehaviour
{
    [SerializeField] private SavingLoadingPageController slpc;
    public AudioSource soundPlayer;
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
    public void PrintContinueGameMessage()
    {
        Debug.Log("ContinueGame!");
        soundPlayer.Play();
        slpc.LoadLatestGame();
    }
    public void PrintViewLoadingMessage()
    {
        Debug.Log("ViewLoading!");
        // SceneManager.LoadScene("LoadPage-inGame");
        LabelController2.toLoadPage_inGame = true;
        LabelController2.from_main_page = true;

        SceneManager.LoadScene("Book2");
        soundPlayer.Play();
    }
    public void PrintSettingMessage()
    {
        Debug.Log("Setting!");
        LabelController2.toSettingPage_inGame = true;
        SceneManager.LoadScene("Book2");
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
