using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneClickManager : MonoBehaviour
{
    private void Update()
    {
        // 鼠標點擊畫面時進入
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainPage");
        }
    }
}
