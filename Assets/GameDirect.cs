using UnityEngine;
using UnityEngine.UI;

public class GameDirect : MonoBehaviour
{
    public InputField inputField;
    public Button button;

    private Color normalColor;
    private Color selectedColor;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#F3C986", out normalColor);
        ColorUtility.TryParseHtmlString("#7B633E", out selectedColor);

        
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        SetButtonColor(selectedColor);
        inputField.text = "";
        SetInputFieldColor(normalColor);
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            SetButtonColor(normalColor);
            SetInputFieldColor(selectedColor);
        }
    }

    private void SetButtonColor(Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }

    private void SetInputFieldColor(Color color)
    {
        inputField.targetGraphic.color = color;
    }
}
