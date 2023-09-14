using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private Button[] buttons;


    void Start()
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            AudioSource audioSource = audioSources[i];

            buttons[i].onClick.AddListener(() =>
            {
                audioSource.Play();
            });
        }

    }


}