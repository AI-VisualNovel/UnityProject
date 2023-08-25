using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    [SerializeField] private CanvasGroup LeftText;
    [SerializeField] private CanvasGroup RightText;
    [SerializeField] private bool StartFade = false;
    
    public void FadeIn() {
        // LeftText.alpha = 1;
        // RightText.alpha = 0.5f;
        StartFade = true;
    }

    private void Update() {
        if (StartFade) {
            if (LeftText.alpha < 1 && RightText.alpha < 1) {
                LeftText.alpha += Time.deltaTime;
                RightText.alpha += Time.deltaTime;
                if (LeftText.alpha >= 1 && RightText.alpha >= 1) {
                    StartFade =  false;
                }
            }
        }
    }
}
