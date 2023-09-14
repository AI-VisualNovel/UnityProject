using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAudioController : MonoBehaviour
{
    [SerializeField] private Slider[] sliders;
    [SerializeField] private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Slider slider in sliders)
        {
            slider.onValueChanged.AddListener(playSound);
        }
    }

    // Update is called once per frame
    private void playSound(float value)
    {
        audio.Play();
    }
}
