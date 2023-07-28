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

    void Start()
    {
        // 設置初始音量
        BackgroundSlideraudioSource.volume = BackgroundSlider.value;
        EffectMusicSlideraudioSource.volume = EffectMusicSlider.value;
        CharacterDubbingSlideraudioSource.volume = CharacterDubbingSlider.value;
    }

    public void OnBackgroundVolumeChanged(float volume)
    {
        // 當 BackgroundSlider 值改變時掉用此方法
        BackgroundSlideraudioSource.volume = volume;
    }
    

    public void OnEffectMusicVolumeChanged(float volume)
    {
        // 當 EffectMusicSlider 值改變時掉用此方法
        EffectMusicSlideraudioSource.volume = volume;
    }

    public void OnCharacterDubbingVolumeChanged(float volume)
    {
        // 當 CharacterDubbingSlider 值改變時掉用此方法
        CharacterDubbingSlideraudioSource.volume = volume;
    }
}