using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InputFieldManager : MonoBehaviour
{
    public static string user_api;
    public TMP_InputField inputField;
    [SerializeField] private AudioSource ClickSound;

    private void Start()
    {
        
    }

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
            Debug.Log("InputField 的文本为：" + user_api);
            SceneManager.LoadScene("GamePage");

        }
    }
}