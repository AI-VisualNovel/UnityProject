using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LinkClickHandler : MonoBehaviour
{
    public void OnLinkClick(string linkID)
    {
        Application.OpenURL(linkID);
    }
    public void OpenGmail()
    {
        Application.OpenURL("mailto:ronni31027@g.ncu.edu.tw");
    }
}
