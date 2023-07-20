using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TextGenerationOptionManager : MonoBehaviour
{

    public Button[] buttons;
    public Button selectedButton;
    [SerializeField] private AudioSource clickEffect;

    private void Start()
    {
        // 初始化按钮状态
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => SelectButton(button));
        }
        
    }

    private void SelectButton(Button button)
    {
        clickEffect.Play();
        // 如果按钮已选中，则不执行任何操作
        if (button == selectedButton)
            return;

        // 取消之前选中的按钮状态
        if (selectedButton != null)
        {
            Color normal;
            ColorUtility.TryParseHtmlString("#F3C986", out normal);
            ColorBlock colors = selectedButton.colors;
            colors.normalColor = normal; 
            selectedButton.colors = colors;
        }

        Color selected;
        ColorUtility.TryParseHtmlString("#7B633E", out selected);
        ColorBlock selectedColors = button.colors;
        selectedColors.normalColor = selected; // 设置选中状态的颜色
        button.colors = selectedColors;
        selectedButton = button;
    }
    public string GetSelectedButtonText()
    {
        if (selectedButton != null)
        {
            return selectedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        } else {
            return "null";
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 检查是否点击在按钮之外
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // 如果点击在按钮之外，则不执行任何操作
                return;
            }
        }
    }
}
