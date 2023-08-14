using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ScreenShotImage1;
    public GameObject ScreenShotImage2;
    public GameObject ArrowImage;
    public GameObject FlippedImage;

    private void Start()
    {
        ScreenShotImage1.SetActive(false);
        ScreenShotImage2.SetActive(false);
        ArrowImage.SetActive(false);
        FlippedImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScreenShotImage1.SetActive(true);
        ScreenShotImage2.SetActive(true);
        ArrowImage.SetActive(true);
        FlippedImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScreenShotImage1.SetActive(false);
        ScreenShotImage2.SetActive(false);
        ArrowImage.SetActive(false);
        FlippedImage.SetActive(false);
    }
}
