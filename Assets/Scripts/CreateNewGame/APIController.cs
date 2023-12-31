using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class APIController : MonoBehaviour
{
    public string realText = "";
    public string maskedText = "";
    [SerializeField] public InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private Sprite EyeSprite;
    [SerializeField] private Sprite EyeSalshSprite;

    private bool isMasked = true;

    void Start()
    {
        realText = PlayerPrefs.GetString("APIKey", "");
        maskedText = new string('*', realText.Length);
        inputField.text = maskedText;
        inputField.onValueChanged.AddListener(OnValueChange);
        button.onClick.AddListener(TransitInput);
    }

    void OnValueChange(string text)
    {
        if (isMasked)
        {
            if (text.Length > maskedText.Length)
            {
                realText += text[text.Length - 1];
            }
            else if (text.Length < maskedText.Length)
            {
                realText = realText.Substring(0, text.Length);
            }

            maskedText = new string('*', realText.Length);
            inputField.text = maskedText;
            inputField.caretPosition = maskedText.Length;
        }
        else
        {
            realText = text;
            maskedText = new string('*', realText.Length);
        }
        realText = RemoveSpecialCharacters(realText);
        PlayerPrefs.SetString("APIKey", realText);
        PlayerPrefs.Save();
    }



    public void TransitInput()
    {
        Image buttonImage = button.GetComponent<Image>();

        inputField.onValueChanged.RemoveListener(OnValueChange);

        if (isMasked)
        {
            buttonImage.sprite = EyeSalshSprite;
            inputField.text = realText;
            isMasked = false;
        }
        else
        {
            buttonImage.sprite = EyeSprite;
            inputField.text = maskedText;
            inputField.caretPosition = maskedText.Length;
            isMasked = true;
        }

        inputField.onValueChanged.AddListener(OnValueChange);
    }
    public string GetrealText()
    {
        return realText;
    }
    public static string RemoveSpecialCharacters(string input)
    {
        return Regex.Replace(input, @"[^a-zA-Z0-9\-]+", "", RegexOptions.Compiled);
    }

}
