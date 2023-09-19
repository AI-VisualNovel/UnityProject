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
    public Image foreceValueBar;
    public Image wisdomValueBar;
    public Image infoValueBar;
    public Image fameValueBar;

    public GameObject getValueTipPanel;
    public float forceValue = 0f;
    public float wisdomValue = 0f;
    public float infoValue = 0f;
    public float fameValue = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreceValueBar.fillAmount = forceValue / 150f;
        wisdomValueBar.fillAmount = wisdomValue / 150f;
        infoValueBar.fillAmount = infoValue / 150f;
        fameValueBar.fillAmount = fameValue / 150f;
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

    public string EndJudgment(int plotIndex){
        string theEndPrompt = "";
        print(forceValue + " " + wisdomValue + " " + infoValue + " " + fameValue);
        switch(plotIndex)
        {
                case 0:
                    if(forceValue >= 100){
                        theEndPrompt = "成功達成「參與武林大會擊敗所有人成為第一名」這個遊戲目標";
                    }else{
                        theEndPrompt = "沒有達成「參與武林大會擊敗所有人成為第一名」這個遊戲目標，參加後敗給了某個對手";
                    }
                    break;
                case 1:
                    if(forceValue >= 20 && wisdomValue >= 50 && fameValue >= 80){
                        theEndPrompt = "成功達成「成為朝廷的官員」這個遊戲目標";
                    }else{
                        theEndPrompt = "沒有達成「成為朝廷的官員」這個遊戲目標，一生都是平民";
                    }
                    break;
                case 2:
                    if(forceValue >= 70 && wisdomValue >= 20 && infoValue >= 50){
                        theEndPrompt = "成功達成「找到仇人並擊敗他成功復仇」這個遊戲目標";
                    }else{
                        if(infoValue < 50){
                            theEndPrompt = "沒有達成「找到仇人並擊敗他成功復仇」這個遊戲目標，根本沒有找到仇人";
                        }else{
                        theEndPrompt = "沒有達成「找到仇人並擊敗他成功復仇」這個遊戲目標，找到仇人後被他擊敗";
                        }
                    }
                    break;
                case 3:
                    if(forceValue >= 60 && wisdomValue >= 20 && infoValue >= 70){
                        theEndPrompt = "成功達成「找友人解救他並擊敗綁架他的人」這個遊戲目標";
                    }else{
                        if(infoValue < 50){
                            theEndPrompt = "沒有達成「找友人解救他並擊敗綁架他的人」這個遊戲目標，根本沒有找到友人";
                        }else{
                            theEndPrompt = "沒有達成「找友人解救他並擊敗綁架他的人」這個遊戲目標，找到友人後被敵方擊敗";
                        }
                    }
                    break;
                case 4:
                    if(forceValue >= 100 && fameValue >= 60){
                        theEndPrompt = "成功達成「擊敗所有勢力」這個遊戲目標，一統江湖";
                    }else{
                        theEndPrompt = "沒有達成「擊敗所有勢力」這個遊戲目標，在武林中沒沒無聞的活下去";
                    }
                    break;
                case 5:
                    if(forceValue >= 80 && wisdomValue >= 40 && fameValue >= 50){
                        theEndPrompt = "成功達成「創立自己的門派」這個遊戲目標";
                    }else{
                        theEndPrompt = "沒有達成「創立自己的門派」這個遊戲目標，一個人獨木難支";
                    }
                    break;
                case 6:
                    if(forceValue >= 100 && wisdomValue >= 20 && infoValue >= 50){
                        theEndPrompt = "成功達成「獨自擊敗邪惡」這個遊戲目標";
                    }else{
                        theEndPrompt = "沒有達成「獨自擊敗邪惡」這個遊戲目標，而是被邪惡擊敗了";
                    }
                    break;
                case 7:
                    if(forceValue >= 70 && fameValue >= 30 && infoValue >= 20){
                        theEndPrompt = "成功達成「組織各方勢力擊敗邪惡」這個遊戲目標";
                    }else{
                        theEndPrompt = "沒有達成「組織各方勢力擊敗邪惡」這個遊戲目標，參加後再敗給了某個對手";
                    }
                    break;
                case 8:
                    if(forceValue >= 80 && wisdomValue >= 50){
                        theEndPrompt = "成功達成「擊敗外敵」這個遊戲目標";
                    }else{
                        theEndPrompt = "沒有達成「擊敗外敵」這個遊戲目標，最後被外敵勢力擊敗";
                    }
                    break;
                case 9:
                    if(forceValue >= 60 && wisdomValue >= 60){
                        theEndPrompt = "成功達成「解開層層困難獲得寶藏」這個遊戲目標";
                    }else{
                        theEndPrompt = "沒有達成「解開層層困難獲得寶藏」這個遊戲目標，空手而歸";
                    }
                    break;
            }
            return theEndPrompt;
    }
}
