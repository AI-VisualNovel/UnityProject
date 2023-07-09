using UnityEngine;
using UnityEngine.UI;

public class CreateNewGameButton : MonoBehaviour
{
    public Button[] buttons;
    public Color desiredColor;
    public Toggle[] toggles;
    public InputField inputField;
    [SerializeField] private AudioSource ClickSound;
    public void SearchSelectedButtonText()
    {
        ClickSound.Play();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                string toggleText = toggle.GetComponentInChildren<Text>().text;
                Debug.Log("Selected Toggle Text: " + toggleText);
            }
        }

        Color inputFieldColor = inputField.image.color;
        if (ColorUtility.ToHtmlStringRGB(inputFieldColor) == ColorUtility.ToHtmlStringRGB(desiredColor))
        {
            string inputText = inputField.text;
            Debug.Log("遊戲走向: " + inputText);
        }

        foreach (Button button in buttons)
        {
            ColorBlock buttonColorBlock = button.colors;
            Color buttonNormalColor = buttonColorBlock.normalColor;
            if (ColorUtility.ToHtmlStringRGB(buttonNormalColor) == ColorUtility.ToHtmlStringRGB(desiredColor))
            {
                string buttonText = button.GetComponentInChildren<Text>().text;
                Debug.Log("Selected Button Text: " + buttonText);
            }
        }
    }
}
