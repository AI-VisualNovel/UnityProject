using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Book1HoverReturnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        // 初始時禁用按鈕
        button.interactable = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠標進入時啟用按鈕
        button.interactable = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠標離開時禁用按鈕
        button.interactable = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 鼠標點擊時跳轉到mainpage
        SceneManager.LoadScene("MainPage");
    }
}
