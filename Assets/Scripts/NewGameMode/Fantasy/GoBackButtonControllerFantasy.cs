using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackButtonControllerFantasy : MonoBehaviour
{
    private Button goBackButton;
    private GameObject parentPlace;

    void Start()
    {
        goBackButton = GetComponent<Button>();
        parentPlace = transform.parent.gameObject;
        goBackButton.onClick.AddListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        parentPlace.SetActive(false);
    }
}
