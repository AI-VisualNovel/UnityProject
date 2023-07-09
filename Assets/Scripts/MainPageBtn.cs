using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("HelpPage-main");
    }
    public void PrintContinueGameMessage()
    {
        Debug.Log("ContinueGame!");
    }
    public void PrintViewLoadingMessage()
    {
        Debug.Log("ViewLoading!");
    }
    public void PrintSettingMessage()
    {
        Debug.Log("Setting!");
    }
    public void PrintExitGameMessage()
    {
        Debug.Log("ExitGame!");
    }
}
