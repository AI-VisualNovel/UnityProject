using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompleteButtonManager : MonoBehaviour
{
    public TextGenerationOptionManager textGenerationOptionManager;
    public UILanguageOptionManager uiLanguageOptionManager;
    [SerializeField] private AudioSource ClickSound;


    public void OnButtonClicked()
    {
        ClickSound.Play();

        // 获取 TextGenerationOptionManager 中选中按钮的文本内容
        if (textGenerationOptionManager.GetSelectedButtonText() != null)
        {
            string TextGenerationOptionManager = textGenerationOptionManager.GetSelectedButtonText();
            Debug.Log("Text Generation Option Selected Button Text: " + TextGenerationOptionManager);
        }else {
            Debug.Log("There is no Text Generation Option Button selected");
        }

        // 获取 UILanguageOptionManager 中选中按钮的文本内容
        if (uiLanguageOptionManager.GetSelectedButtonText() != null)
        {
            string UILanguageOptionText = uiLanguageOptionManager.GetSelectedButtonText();
            Debug.Log("UI Language Option Manager Selected Button Text: " + UILanguageOptionText);
        }else {
            Debug.Log("There is no UI Language Option Manager Button selected");
        }
    }

}



