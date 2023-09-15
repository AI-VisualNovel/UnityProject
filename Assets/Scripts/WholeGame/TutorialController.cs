using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialPanels;
    [SerializeField] private GameObject StoryController;


    // Start is called before the first frame update
    void Start()
    {
        tutorialPanels[0].SetActive(true);
        for (int i = 0; i < tutorialPanels.Count; i++)
        {
            int index = i;
            tutorialPanels[i].GetComponent<Button>().onClick.AddListener(() => ShowNextTutorial(index));
        }
    }


    private void ShowNextTutorial(int tutorialIndex)
    {
        tutorialPanels[tutorialIndex].SetActive(false);
        if (tutorialIndex + 1 < tutorialPanels.Count)
        {
            tutorialPanels[tutorialIndex + 1].SetActive(true);
        }
        else
        {
            StoryController.SetActive(true);
        }
    }
}
