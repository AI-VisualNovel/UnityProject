using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SettingEffectController : MonoBehaviour
{
    [SerializeField] public AudioSource[] BGMSource;
    [SerializeField] public AudioSource[] SoundEffectSource;
    [SerializeField] public Image targetPanel;


    void Update()
    {
        AdjustVolume();
        AdjustTransparency();
    }

    public void AdjustVolume()
    {
        foreach (AudioSource source in BGMSource)
        {
            if (source)
            {
                source.volume = PlayerPrefs.GetFloat("BGMValue");
            }
            else
            {
                Debug.LogWarning("某些BGM可能未正確設定或缺失!");
            }
        }
        foreach (AudioSource source in SoundEffectSource)
        {
            if (source)
            {
                source.volume = PlayerPrefs.GetFloat("SoundEffectValue");
            }
            else
            {
                Debug.LogWarning("某些SoundEffect可能未正確設定或缺失!");
            }
        }
    }
    public void AdjustTransparency()
    {
        Color currentColor = targetPanel.color;
        currentColor.a = PlayerPrefs.GetFloat("DTValue");
        targetPanel.color = currentColor;
    }
}
