using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class LabelSceneChanging : MonoBehaviour
{
    public void LoadPage() {  
        SceneManager.LoadScene("LoadPage-main");  
    }  
    public void SettingPage() {  
        SceneManager.LoadScene("SettingPage-main");  
    }  
    public void AboutPage() {  
        SceneManager.LoadScene("AboutPage-main");  
    }  
    public void HelpPage() {  
        SceneManager.LoadScene("HelpPage-main");  
    }  
    public void HistoryPage() {  
        SceneManager.LoadScene("HistoryPage");  
    }  
    public void SavePage() {  
        SceneManager.LoadScene("SavePage");  
    }  
    public void MainPage() {  
        SceneManager.LoadScene("MainPage");  
    }  
    
}
