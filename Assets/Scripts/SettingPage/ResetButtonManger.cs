using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonManger : MonoBehaviour
{
    [SerializeField] private AudioSource ClickSound;

    public void OnButtonClicked()
    {
        ClickSound.Play();
        
    }
}
