using UnityEngine;
using UnityEngine.UI;

public class GameLoadingControl : MonoBehaviour
{
    public RawImage loadingImage;
    public Slider loadingSlider;
    public Text loadingSliderText;

    private float totalTime = 5f; // 预计加载时间（秒）
    private float increment; // 每帧应该增加的进度
    private float currentProgress = 0f; // 当前进度

    private bool isLoading = false; // 控制加载状态
    private bool accelerate = false; // 标志是否加速

    private float elapsedTime = 0f; // 计时器
    private bool slowDown = false; // 标志是否减速
    
    private void Start()
    {
        increment = 2000f / (totalTime * 60f); // 假设每秒运行60帧
        loadingImage.gameObject.SetActive(false); // 初始时隐藏图片和Slider
        loadingSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isLoading && currentProgress < 100f)
        {
            if (accelerate)
            {
                // 加速增加進度
                currentProgress += increment * 20f * Time.deltaTime;
            }
            else if (slowDown)
            {
                currentProgress += increment * 0.5f * Time.deltaTime; // 减速增加进度
            }
            else
            {
                // 等速增加進度
                currentProgress += increment * Time.deltaTime;
            }

            loadingSlider.value = currentProgress / 100f;
            loadingSliderText.text = Mathf.FloorToInt(currentProgress).ToString() + "%";
            if (currentProgress >= 100f)
            {
                // 隱藏Slider和RawImage和text
                loadingSlider.gameObject.SetActive(false);
                loadingImage.gameObject.SetActive(false);
                loadingSliderText.gameObject.SetActive(false);
            }
            if (isLoading)
            {
                elapsedTime += Time.deltaTime;

                // 当时间超过五秒时，启动减速
                if (elapsedTime >= 5f && !accelerate)
                {
                    slowDown = true;
                }
            }
        }
    }

    public void StartLoading()
    {
        Debug.Log("StartLoading method called");
        isLoading = true;
        currentProgress = 0f;
        loadingSlider.value = 0f;
        loadingSliderText.text = "0%";
        loadingSlider.gameObject.SetActive(true);
        loadingImage.gameObject.SetActive(true);
        loadingSliderText.gameObject.SetActive(true);

        // 在这里开始加载数据或执行异步操作

        // 模拟获得回傳值后加速
        // chatgpt有回傳資料時開始加速
        Invoke("AccelerateProgress", 1f); // 2秒后加速
    }
    // 模擬
    private void AccelerateProgress()
    {
        accelerate = true;
    }
}
