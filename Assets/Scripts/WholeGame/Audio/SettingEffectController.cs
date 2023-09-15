using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SettingEffectController : MonoBehaviour
{
    [SerializeField] public AudioSource BGMSource;
    [SerializeField] public AudioSource[] SoundEffectSource;
    [SerializeField] public Image targetPanel;
    [SerializeField] private float bgmMaxVolume = 0.5f;
    [SerializeField] private int BGSoundLong;


    public float fadeInDuration = 2.0f;
    public float fadeOutDuration = 2.0f;

    private bool isFading = false;
    private float currentMaxVolume;
    private void Start()
    {
        BGMSource.loop = false;

        StartAudioLoop();
    }
    void Update()
    {
        currentMaxVolume = PlayerPrefs.GetFloat("BGMValue");


        bgmMaxVolume = currentMaxVolume;
        BGMSource.volume = bgmMaxVolume;

        AdjustVolume();
        AdjustTransparency();
    }

    public void AdjustVolume()
    {
        if (!isFading)
        {
            BGMSource.volume = PlayerPrefs.GetFloat("BGMValue");
        }

        // if (BGMSource)
        // {
        //     BGMSource.volume = PlayerPrefs.GetFloat("BGMValue");
        // }
        // else
        // {
        //     Debug.LogWarning("某些BGM可能未正確設定或缺失!");
        // }

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
        if (targetPanel)
        {
            Color currentColor = targetPanel.color;
            currentColor.a = PlayerPrefs.GetFloat("DTValue");
            targetPanel.color = currentColor;
        }
    }




    private void StartAudioLoop()
    {
        StartCoroutine(AudioLoop());
    }

    private IEnumerator AudioLoop()
    {
        while (true)
        {
            BGMSource.Play();

            isFading = true;

            float fadeInTime = 0;
            while (fadeInTime < fadeInDuration)
            {
                fadeInTime += Time.deltaTime;
                BGMSource.volume = Mathf.Lerp(0, bgmMaxVolume, fadeInTime / fadeInDuration);
                yield return null;
            }

            yield return new WaitForSeconds(BGSoundLong - fadeInDuration - fadeOutDuration);

            float fadeOutTime = 0;
            while (fadeOutTime < fadeOutDuration)
            {
                fadeOutTime += Time.deltaTime;
                BGMSource.volume = Mathf.Lerp(bgmMaxVolume, 0, fadeOutTime / fadeOutDuration);
                yield return null;
            }

            isFading = false;
        }
    }
}
