using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ResetButtonManager : MonoBehaviour
{    
    [SerializeField] private AudioSource ClickSound;
    public Button[] buttons;
    public TMP_InputField inputField;
    public float defaultBackgroundSliderValue;
    public float defaultEffectMusicSliderValue;
    public float defaultCharacterDubbingSliderValue;

    Color normalColor;
    Color selectedColor;


    public void OnButtonClicked()
    {
        ClickSound.Play();

        // button
        foreach (Button button in buttons)
        {
            ColorBlock colors = button.colors;
            ColorUtility.TryParseHtmlString("#F3C986", out normalColor);
            colors.normalColor = normalColor;
            button.colors = colors;
        }
        
        // inputfield
        inputField.text = string.Empty;

        // 通過 FindObjectOfType 找到 UILanguageOptionManager 的實例
        UILanguageOptionManager uiLanguageOptionManager = FindObjectOfType<UILanguageOptionManager>();
        TextGenerationOptionManager textgenerationOptionManager = FindObjectOfType<TextGenerationOptionManager>();

        if (uiLanguageOptionManager != null)
        {
            // 調用 SelectButton 方法並傳遞 null 作為參數，for真正還原
            uiLanguageOptionManager.SelectButton(null);
        }
        if (textgenerationOptionManager != null)
        {
            // 調用 SelectButton 方法並傳遞 null 作為參數，for真正還原
            textgenerationOptionManager.SelectButton(null);
        }

        // slide
        VolumeControlManager volumeControlManager = GameObject.FindObjectOfType<VolumeControlManager>();
        // slider 的滑塊
        volumeControlManager.BackgroundSlider.value = defaultBackgroundSliderValue;
        volumeControlManager.EffectMusicSlider.value = defaultEffectMusicSliderValue;
        volumeControlManager.CharacterDubbingSlider.value = defaultCharacterDubbingSliderValue;
        // slider 的音頻源
        volumeControlManager.BackgroundSlideraudioSource.volume = defaultBackgroundSliderValue;
        volumeControlManager.EffectMusicSlideraudioSource.volume = defaultEffectMusicSliderValue;
        volumeControlManager.CharacterDubbingSlideraudioSource.volume = defaultCharacterDubbingSliderValue;

        
    }
}
