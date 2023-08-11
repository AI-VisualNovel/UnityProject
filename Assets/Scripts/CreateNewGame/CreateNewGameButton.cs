using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateNewGameButton : MonoBehaviour
{
    public Button[] buttons;
    public Color desiredColor;
    public Toggle[] toggles;
    public InputField inputField;
    [SerializeField] private AudioSource ClickSound;
    
    public static string[] buttonTexts;
    public static int buttonLength;
    public static string gamedir;
    public void SearchSelectedButtonText()
    {
        buttonLength = buttons.Length;
        buttonTexts = new string[buttons.Length];
        buttonTexts[0] = "白色恐怖";
        buttonTexts[1] = "漫畫";
        buttonTexts[2] = "中";
        buttonTexts[3] = "無";
        buttonTexts[4] = "中文";
        int i = 0;
        ClickSound.Play();
        // foreach (Toggle toggle in toggles)
        // {
        //     if (toggle.isOn)
        //     {
        //         string toggleText = toggle.GetComponentInChildren<Text>().text;
        //         Debug.Log("Selected Toggle Text: " + toggleText);
        //     }
        // }

        Color inputFieldColor = inputField.image.color;
        if (ColorUtility.ToHtmlStringRGB(inputFieldColor) == ColorUtility.ToHtmlStringRGB(desiredColor))
        {
            string inputText = inputField.text;
            gamedir = inputText;
            Debug.Log("遊戲走向: " + inputText);
        }

        foreach (Button button in buttons)
        {
            ColorBlock buttonColorBlock = button.colors;
            Color buttonNormalColor = buttonColorBlock.normalColor;
            if (ColorUtility.ToHtmlStringRGB(buttonNormalColor) == ColorUtility.ToHtmlStringRGB(desiredColor))
            {
                string buttonText = button.GetComponentInChildren<Text>().text;
                // Debug.Log("Selected Button Text: " + buttonText);
                buttonTexts[i] = buttonText;
                Debug.Log("Selected Button Text: " + buttonTexts[i]);
                i++;
            }
        }
    }
}
