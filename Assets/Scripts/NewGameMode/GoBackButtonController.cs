using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackButtonController : MonoBehaviour
{
    private Button goBackButton;
    private GameObject parentPlace;
    [SerializeField] private AudioSource exploreBackgroundSound;

    void Start()
    {
         

        goBackButton = GetComponent<Button>();
        parentPlace = transform.parent.gameObject;
        goBackButton.onClick.AddListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
         AudioClip newSoundClip = Resources.Load<AudioClip>("GameMusic/WuXia/" + 1); 
            exploreBackgroundSound.clip = newSoundClip;
            exploreBackgroundSound.enabled = true;
            exploreBackgroundSound.Play();
        parentPlace.SetActive(false);
    }
}
