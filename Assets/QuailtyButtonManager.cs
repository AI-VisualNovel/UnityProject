// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;

// public class QualityButtonManager : MonoBehaviour
// {
//     public Button[] buttons;

//     private Button selectedButton;

//     private void Start()
//     {
//         foreach (Button button in buttons)
//         {
//             button.onClick.AddListener(() => SelectButton(button));
//         }
//     }

//     private void SelectButton(Button button)
//     {
//         if (button == selectedButton)
//             return;

//         if (selectedButton != null)
//         {
//             Color normal;
//             ColorUtility.TryParseHtmlString("#F3C986", out normal);
//             ColorBlock colors = selectedButton.colors;
//             colors.normalColor = normal; 
//             selectedButton.colors = colors;
//         }

//         Color selected;
//         ColorUtility.TryParseHtmlString("#7B633E", out selected);
//         ColorBlock selectedColors = button.colors;
//         selectedColors.normalColor = selected; // 设置选中状态的颜色
//         button.colors = selectedColors;
//         selectedButton = button;
//     }
//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             // 检查是否点击在按钮之外
//             if (!EventSystem.current.IsPointerOverGameObject())
//             {
//                 // 如果点击在按钮之外，则取消选中状态
//                 if (selectedButton != null)
//                 {
//                     Color selected;
//                     ColorUtility.TryParseHtmlString("#7B633E", out selected);
//                     ColorBlock colors = selectedButton.colors;
//                     colors.normalColor = selected; // 设置未选中状态的颜色
//                     selectedButton.colors = colors;
//                     selectedButton = null;
//                 }
//             }
//         }
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QualityButtonManager : MonoBehaviour
{
    public Button[] buttons;

    private Button selectedButton;

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
