using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LinkClickHandler : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        // audioSource = GetComponent<AudioSource>();
        // audioSource.Stop();
    }

    public void OnLinkClick(string linkID)
    {
        Application.OpenURL(linkID);
    }
    public void OpenGmail()
    {
        Application.OpenURL("mailto:ronni31027@g.ncu.edu.tw");
    }

    public void OnPointerEnter()
    {
        if (audioSource != null)
        {
            Debug.Log(999);
            audioSource.Play();
        }
    }
}
