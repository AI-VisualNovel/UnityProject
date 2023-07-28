using UnityEngine;
using UnityEngine.UI;

public class VolumeControlManager : MonoBehaviour
{
    // 引用音頻源
    public AudioSource BackgroundSlideraudioSource;
    public AudioSource EffectMusicSlideraudioSource;
    public AudioSource CharacterDubbingSlideraudioSource;
    // 引用 Slider 對象
    public Slider BackgroundSlider;
    public Slider EffectMusicSlider;
    public Slider CharacterDubbingSlider;
    // 用來顯示Slider音量的Text元件
    public Text BackgroundVolumeText;
    public Text EffectMusicVolumeText;
    public Text CharacterDubbingVolumeText;
    // 點選的時候會有聲音
    [SerializeField] private AudioSource ClickSound;

    void Start()
    {
        // 設置初始音量
        BackgroundSlideraudioSource.volume = BackgroundSlider.value;
        EffectMusicSlideraudioSource.volume = EffectMusicSlider.value;
        CharacterDubbingSlideraudioSource.volume = CharacterDubbingSlider.value;
        // 顯示初始音量數值
        UpdateVolumeText(BackgroundSlider, BackgroundVolumeText);
        UpdateVolumeText(EffectMusicSlider, EffectMusicVolumeText);
        UpdateVolumeText(CharacterDubbingSlider, CharacterDubbingVolumeText);
    }

    public void OnBackgroundVolumeChanged(float volume)
    {
        ClickSound.Play();
        // 當 BackgroundSlider 值改變時掉用此方法
        BackgroundSlideraudioSource.volume = volume;
         // 顯示Slider音量數值
        UpdateVolumeText(BackgroundSlider, BackgroundVolumeText);
    }
    

    public void OnEffectMusicVolumeChanged(float volume)
    {
        ClickSound.Play();
        // 當 EffectMusicSlider 值改變時掉用此方法
        EffectMusicSlideraudioSource.volume = volume;
        // 顯示Slider音量數值
        UpdateVolumeText(EffectMusicSlider, EffectMusicVolumeText);
    }

    public void OnCharacterDubbingVolumeChanged(float volume)
    {
        ClickSound.Play();
        // 當 CharacterDubbingSlider 值改變時掉用此方法
        CharacterDubbingSlideraudioSource.volume = volume;
        // 顯示Slider音量數值
        UpdateVolumeText(CharacterDubbingSlider, CharacterDubbingVolumeText);
    }
    
    private void UpdateVolumeText(Slider slider, Text volumeText)
    {
        // 取得Slider的值
        float volume = slider.value;

        // 將值轉換成百分比格式，並設定給Text元件
        volumeText.text = (volume * 100f).ToString("F1") + "%";
    }
}



