using UnityEngine;
using TMPro;

public class InputFieldManager : MonoBehaviour
{
    public TMP_InputField inputField;
    [SerializeField] private AudioSource ClickSound;

    private void Start()
    {
        
    }

    public void OnButtonClicked()
    {
        ClickSound.Play();
        string text = inputField.text;
        if (string.IsNullOrEmpty(text))
        {
            Debug.Log("用戶未輸入任何訊息");
        }
        else
        {
            Debug.Log("InputField 的文本为：" + text);
        }
    }
}
