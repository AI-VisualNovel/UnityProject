using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainPageBtn : MonoBehaviour
{
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
        SceneManager.LoadScene("CreateNewGamePage");
        soundPlayer.Play();
    }
    public void PrintContinueGameMessage()
    {
        Debug.Log("ContinueGame!");
        soundPlayer.Play();
    }
    public void PrintViewLoadingMessage()
    {
        Debug.Log("ViewLoading!");
        SceneManager.LoadScene("LoadPage-inGame");
        soundPlayer.Play();
    }
    public void PrintSettingMessage()
    {
        Debug.Log("Setting!");
        SceneManager.LoadScene("SettingPage-inGame");
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
