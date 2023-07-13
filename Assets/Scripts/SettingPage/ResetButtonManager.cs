using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ResetButtonManager : MonoBehaviour
{    
    [SerializeField] private AudioSource ClickSound;
    public Button[] buttons;
    public TMP_InputField inputField;

    Color normalColor;
    Color selectedColor;

    public void OnButtonClicked()
    {
        ClickSound.Play();
        // button
        foreach (Button button in buttons)
        {
            ColorBlock colors = button.colors;
            ColorUtility.TryParseHtmlString("#F3C986", out normalColor);
            colors.normalColor = normalColor;
            button.colors = colors;
        }
        
        // inputfield
        inputField.text = string.Empty;
        
    }
}
