using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider soundEffectSlider;
    public Slider dTSlider;

    private void Start()
    {
        // 從PlayerPrefs中讀取之前保存的值，如果不存在，則使用預設值
        bgmSlider.value = PlayerPrefs.GetFloat("BGMValue", 0.5f);  // 使用0.5作為預設值
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffectValue", 0.5f);  // 使用0.5作為預設值
        dTSlider.value = PlayerPrefs.GetFloat("DTValue", 0.5f);  // 使用0.5作為預設值
        
        // 註冊Slider的值變更事件
        bgmSlider.onValueChanged.AddListener(bgmOnSliderValueChanged);
        soundEffectSlider.onValueChanged.AddListener(soundEffectOnSliderValueChanged);
        dTSlider.onValueChanged.AddListener(dTOnSliderValueChanged);
    }

    private void bgmOnSliderValueChanged(float newValue)
    {
        // 更新bgmValue
        PlayerPrefs.SetFloat("BGMValue", newValue);
        PlayerPrefs.Save();
        print(PlayerPrefs.GetFloat("BGMValue"));
    }

    private void soundEffectOnSliderValueChanged(float newValue)
    {
        // 更新soundEffectValue
        PlayerPrefs.SetFloat("SoundEffectValue", newValue);
        PlayerPrefs.Save();
        print(PlayerPrefs.GetFloat("SoundEffectValue"));
    }

    private void dTOnSliderValueChanged(float newValue)
    {
        // 更新dTValue
        PlayerPrefs.SetFloat("DTValue", newValue);
        PlayerPrefs.Save();
        print(PlayerPrefs.GetFloat("DTValue"));
    }
}
