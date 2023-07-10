using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LabelManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Label;

    public void OnPointerEnter(PointerEventData eventData){
        // Label.SetHighlight(eventData);
         Debug.Log("Mouse is over GameObject.");
    }

    public void OnPointerExit(PointerEventData eventData){
        Debug.Log("Mouse is leaving GameObject.");
    }
}
