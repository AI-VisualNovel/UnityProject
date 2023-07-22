using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InputFieldManager : MonoBehaviour
{

    public static string user_api;
    public TMP_InputField inputField;
    [SerializeField] private AudioSource ClickSound;


    public void OnButtonClicked()
    {
        ClickSound.Play();
        user_api= inputField.text;
        if (string.IsNullOrEmpty(user_api))
        {
            Debug.Log("用戶未輸入任何訊息");
        }
        else
        {
            Debug.Log("APIKey 為：" + user_api);
            SceneManager.LoadScene("CreateNewGamePage");

        }
    }
}
