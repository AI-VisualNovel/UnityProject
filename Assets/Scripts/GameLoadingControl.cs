using UnityEngine;
using UnityEngine.UI;

public class GameLoadingControl : MonoBehaviour
{
    // 引用 Slider 對象
    public Slider LoadingSlider;
    // 用來顯示Slider音量的Text元件
    public Text LoadingSliderText;
    // 控制進度
    private float VolumeValue;
    // 遊戲預計運行時間（秒）
    private float totalTime = 5f;
    // 每幀應該增加的進度
    private float increment; 
    // Start is called before the first frame update
    void Start()
    {
        // 計算每幀應該增加的進度
        increment = 100f / (totalTime * 60f); // 假設每秒運行60幀
    }

    // Update is called once per frame
    void Update()
    {
        if (VolumeValue < 100)
        {
            VolumeValue += increment;
            // 實時更新進度百分比的文本顯示
            LoadingSliderText.text = Mathf.FloorToInt(VolumeValue).ToString() + "%";
            LoadingSlider.value = VolumeValue / 100f;
        }
        // else
        // {
        //     LoadingSliderText.text = "Done";
        // }
    }
    private void UpdateVolumeText(Slider slider, Text volumeText)
    {
        // 取得Slider的值
        float volume = slider.value;

        // 將值轉換成百分比格式，並設定給Text元件
        volumeText.text = volume.ToString("F1") + "%";
    }
}
