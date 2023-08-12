using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverReturnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        // 初始时禁用按钮
        button.interactable = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标进入时启用按钮
        button.interactable = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开时禁用按钮
        button.interactable = false;
    }
}
