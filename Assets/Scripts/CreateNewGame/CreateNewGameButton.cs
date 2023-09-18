using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateNewGameButton : MonoBehaviour
{
    public Button[] buttons;
    public Color desiredColor;
    public Toggle[] toggles;
    [SerializeField] private InputField UserInput;
    [SerializeField] private AudioSource ClickSound;
    [SerializeField] private Button FantasyBTN;
    [SerializeField] private Button GhostBTN;
    [SerializeField] private GameObject exploreText;


    [SerializeField] private Button explore;



    public static string[] buttonTexts;
    public static int buttonLength;
    public static string gamedir;
    public static string User_API = "";

    public void Update()
    {
        ColorBlock FantasybuttonColorBlock = FantasyBTN.colors;
        ColorBlock GhostbuttonColorBlock = GhostBTN.colors;

        Color FantasybuttonNormalColor = FantasybuttonColorBlock.normalColor;
        Color GhostbuttonNormalColor = GhostbuttonColorBlock.normalColor;

        if (ColorUtility.ToHtmlStringRGB(FantasybuttonNormalColor) == ColorUtility.ToHtmlStringRGB(desiredColor) || ColorUtility.ToHtmlStringRGB(GhostbuttonNormalColor) == ColorUtility.ToHtmlStringRGB(desiredColor))
        {
            explore.gameObject.SetActive(false);
            exploreText.SetActive(false);
        }
        else
        {
            explore.gameObject.SetActive(true);
            exploreText.SetActive(true);

        }
    }
    public void SearchSelectedButtonText()
    {
        buttonLength = buttons.Length;
        buttonTexts = new string[buttons.Length];
        buttonTexts[0] = "武俠";
        buttonTexts[1] = "奇幻";
        buttonTexts[2] = "靈異";
        buttonTexts[3] = "探索";
        buttonTexts[4] = "永無止盡";
        int i = 0;
        ClickSound.Play();



        User_API = UserInput.text;
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
        if (buttonTexts[1] == "永無止盡")
        {
            switch (buttonTexts[0])
            {
                case "武俠":
                    SceneManager.LoadScene("Legacy_WuXia");
                    break;
                case "奇幻":
                    SceneManager.LoadScene("Legacy_Fantasy");
                    break;
                case "靈異":
                    SceneManager.LoadScene("Legacy_Ghost");
                    break;
            }

        }
        else
        {
            SceneManager.LoadScene("NewGamePage");
        }

    }
}