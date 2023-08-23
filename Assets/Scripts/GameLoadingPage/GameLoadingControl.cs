using UnityEngine;
using UnityEngine.UI;

public class GameLoadingControl : MonoBehaviour
{
    public Slider loadingSlider;
    public RawImage loadingImage;
    public GameObject SliderRotator;

    private bool isLoading = false;

    private void Start()
    {
        loadingSlider.gameObject.SetActive(false);
        loadingImage.gameObject.SetActive(false);
        SliderRotator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isLoading)
        {
            loadingSlider.value = 20;
        }
    }

    public void StartLoading()
    {
        isLoading = true;
        loadingSlider.gameObject.SetActive(true);
        loadingImage.gameObject.SetActive(true);
        SliderRotator.gameObject.SetActive(true);
    }
}
