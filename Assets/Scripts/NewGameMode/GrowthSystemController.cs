using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class GrowthSystemController : MonoBehaviour
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
    public void GetForceValue(int value){
        string getValueTip="";
        switch (value)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                getValueTip = "你感到你的武功有些微增長";
                break;

            case 5:
            case 6:
            case 7:
                getValueTip = "你感到你的武功有所進展";
                break;

            case 8:
            case 9:
            case 10:
                getValueTip = "你感到你的武功突飛猛進";
                break;
            default:
                getValueTip = "你感到武功沒什麼進步";
                break;
        }
        forceValue += value;
        getValueTipPanel.GetComponentInChildren<Text>().text = getValueTip;
        getValueTipPanel.GetComponent<GetValueTipController>().StartCoroutine("MoveObject");
        print(getValueTip);
    }

    public void GetWisdomValue(int value){
        string getValueTip="";
        switch (value)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                getValueTip = "你感到你的智慧有些微增長";
                break;

            case 5:
            case 6:
            case 7:
                getValueTip = "你感到你好像更聰明了";
                break;

            case 8:
            case 9:
            case 10:
                getValueTip = "你感到自己聰明極了";
                break;
            default:
                getValueTip = "你感到自己變得笨了點";
                break;
        }
        wisdomValue += value;
        getValueTipPanel.GetComponentInChildren<Text>().text = getValueTip;
        getValueTipPanel.GetComponent<GetValueTipController>().StartCoroutine("MoveObject");
        print(getValueTip);
    }

    public void GetInfoValue(int value){
        string getValueTip="";
        switch (value)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                getValueTip = "你好像掌握了一點線索";
                break;

            case 5:
            case 6:
            case 7:
                getValueTip = "你收集到了不少有用的情報";
                break;

            case 8:
            case 9:
            case 10:
                getValueTip = "你感覺快要真相大白了";
                break;
            default:
                getValueTip = "你好像一無所獲";
                break;
        }
        infoValue += value;
        getValueTipPanel.GetComponentInChildren<Text>().text = getValueTip;
        getValueTipPanel.GetComponent<GetValueTipController>().StartCoroutine("MoveObject");
        print(getValueTip);
    }

    public void GetFameValue(int value){
        string getValueTip="";
        switch (value)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                getValueTip = "你的名聲略微傳播";
                break;

            case 5:
            case 6:
            case 7:
                getValueTip = "你的名頭漸漸傳開";
                break;

            case 8:
            case 9:
            case 10:
                getValueTip = "你似乎名聲鵲起";
                break;
            default:
                getValueTip = "你依然默默無聞";
                break;
        }
        fameValue += value;
        getValueTipPanel.GetComponentInChildren<Text>().text = getValueTip;
        getValueTipPanel.GetComponent<GetValueTipController>().StartCoroutine("MoveObject");
        print(getValueTip);
    }
}
