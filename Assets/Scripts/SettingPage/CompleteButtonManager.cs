using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CompleteButtonManager : MonoBehaviour
{
    public TextGenerationOptionManager textGenerationOptionManager;
    public UILanguageOptionManager uiLanguageOptionManager;
    public static string user_api;
    public TMP_InputField inputField;
    [SerializeField] private AudioSource ClickSound;


    public void OnButtonClicked()
    {
        ClickSound.Play();

        // 獲取 TextGenerationOptionManager 中選中按鈕的文本內容
        if (textGenerationOptionManager.GetSelectedButtonText() != null)
        {
            string TextGenerationOptionManager = textGenerationOptionManager.GetSelectedButtonText();
            Debug.Log("Text Generation Option Selected Button Text: " + TextGenerationOptionManager);
        }else {
            Debug.Log("There is no Text Generation Option Button selected");
        }

        // 獲取 UILanguageOptionManager 中選中按鈕的文本內容
        if (uiLanguageOptionManager.GetSelectedButtonText() != null)
        {
            string UILanguageOptionText = uiLanguageOptionManager.GetSelectedButtonText();
            Debug.Log("UI Language Option Manager Selected Button Text: " + UILanguageOptionText);
        }else {
            Debug.Log("There is no UI Language Option Manager Button selected");
        }
         // 獲取 VolumeControlManager 的各音量條 volume
        VolumeControlManager volumeControlManager = GameObject.FindObjectOfType<VolumeControlManager>();

        // 從 VolumeControlManager 中讀取各個 Slider 的音量值
        float backgroundVolume = volumeControlManager.BackgroundSlideraudioSource.volume;
        float effectMusicVolume = volumeControlManager.EffectMusicSlideraudioSource.volume;
        float characterDubbingVolume = volumeControlManager.CharacterDubbingSlideraudioSource.volume;

        // 在控制台輸出各個音量值
        Debug.Log("背景音量值: " + backgroundVolume);
        Debug.Log("效果音量值: " + effectMusicVolume);
        Debug.Log("人物配音音量值: " + characterDubbingVolume);

        user_api= inputField.text;

        PlayerPrefs.SetString("User_API", user_api);
        PlayerPrefs.Save();

        if (string.IsNullOrEmpty(user_api))
        {
            Debug.Log("用戶未輸入任何訊息");
        }
        else
        {
            Debug.Log("APIKey 為：" + user_api);
            SceneManager.LoadScene("CreateNewGamePage");
        }
    
    }

}



