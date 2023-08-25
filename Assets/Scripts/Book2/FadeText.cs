using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FadeText : MonoBehaviour
{
    [SerializeField] private CanvasGroup LeftText;
    [SerializeField] private CanvasGroup RightText;
    [SerializeField] private bool StartFade = false;
    private bool canSwitchScene = false;

    public void FadeIn() {
        StartFade = true;
        Debug.Log(987);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            FadeIn();
        }
        if (StartFade) {
            if (LeftText.alpha < 1 && RightText.alpha < 1) {
                LeftText.alpha += Time.deltaTime;
                RightText.alpha += Time.deltaTime;
                if (LeftText.alpha >= 1 && RightText.alpha >= 1) {
                    StartFade =  false;
                    canSwitchScene = true;
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && canSwitchScene == true) {
            SceneManager.LoadScene("MainPage");
        }
    }
}
