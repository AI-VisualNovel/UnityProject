using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class GrowthSystemControllerFantasy : MonoBehaviour
{

    public GameObject getValueTipPanel;
    private int forceValue = 0;
    private int wisdomValue = 0;
    private int infoValue = 0;
    private int fameValue = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetForceValue(){
        int value = UnityEngine.Random.Range(1,11);
        string getValueTip="";
        switch (value)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                getValueTip = "你感到你的能力值有些微增長";
                break;

            case 5:
            case 6:
            case 7:
                getValueTip = "你感到你的能力值有所進展";
                break;

            case 8:
            case 9:
            case 10:
                getValueTip = "你感到你的能力值突飛猛進";
                break;
            default:
                break;
        }
        forceValue += value;
        getValueTipPanel.GetComponentInChildren<Text>().text = getValueTip;
        getValueTipPanel.GetComponent<GetValueTipController>().StartCoroutine("MoveObject");
        print(getValueTip);
    }
}
