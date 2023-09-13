using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialFixedPlacesButton : MonoBehaviour
{
    [SerializeField] private List<Button> placeButtons;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < placeButtons.Count; i++)
        {
            transform.GetChild(i).GetComponentInChildren<Text>().text = placeButtons[i].GetComponentInChildren<Text>().text;
        }        
    }
}
