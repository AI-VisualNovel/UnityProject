using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainPageBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PrintNewGameMessage()
    {
        Debug.Log("NewGame!");
        SceneManager.LoadScene("CreateNewGamePage");
    }
    public void PrintContinueGameMessage()
    {
        Debug.Log("ContinueGame!");
    }
    public void PrintViewLoadingMessage()
    {
        Debug.Log("ViewLoading!");
        SceneManager.LoadScene("LoadPage-inGame");
    }
    public void PrintSettingMessage()
    {
        Debug.Log("Setting!");
        SceneManager.LoadScene("SettingPage-inGame");
    }
    public void PrintExitGameMessage()
    {
        Debug.Log("ExitGame!");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
