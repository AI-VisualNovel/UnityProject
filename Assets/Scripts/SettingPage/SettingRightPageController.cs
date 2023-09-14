using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingRightPageController : MonoBehaviour
{
    public InputField apiInputField;
    public Button resetButton;

    public Slider bgmSlider;
    public Slider soundEffectSlider;
    public Slider dTSlider;

    void Start()
    {
        resetButton.onClick.AddListener(ResetSetting);
    }
    void ResetSetting()
    {
        PlayerPrefs.SetFloat("BGMValue", 0.5f);
        PlayerPrefs.SetFloat("SoundEffectValue", 0.5f);
        PlayerPrefs.SetFloat("DTValue", 0.5f);
        PlayerPrefs.SetString("APIKey", "");
        PlayerPrefs.Save();
        bgmSlider.value = PlayerPrefs.GetFloat("BGMValue", 0.5f);  // 使用0.5作為預設值
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffectValue", 0.5f);  // 使用0.5作為預設值
        dTSlider.value = PlayerPrefs.GetFloat("DTValue", 0.5f);  // 使用0.5作為預設值
        apiInputField.text = PlayerPrefs.GetString("APIKey", "");
    }
}
