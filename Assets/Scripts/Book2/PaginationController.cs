using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;


public class PaginationController : MonoBehaviour
{

    // paginations
    public Button SavePage_1;
    public Button SavePage_2;
    public Button SavePage_3;

    public Button LoadPage_1;
    public Button LoadPage_2;
    public Button LoadPage_3;

    // saveload_screenshots
    public GameObject SavePage_saveload1;
    public GameObject SavePage_saveload2;
    public GameObject SavePage_saveload3;

    public GameObject LoadPage_saveload1;
    public GameObject LoadPage_saveload2;
    public GameObject LoadPage_saveload3;



    // Start is called before the first frame update
    void Start()
    {
        SavePage_saveload2.SetActive(false);
        SavePage_saveload3.SetActive(false);
        LoadPage_saveload2.SetActive(false);
        LoadPage_saveload3.SetActive(false);

        SavePage_1.onClick.AddListener(() => switch_page(SavePage_saveload1, SavePage_saveload2, SavePage_saveload3));
        SavePage_2.onClick.AddListener(() => switch_page(SavePage_saveload2, SavePage_saveload1, SavePage_saveload3));
        SavePage_3.onClick.AddListener(() => switch_page(SavePage_saveload3, SavePage_saveload1, SavePage_saveload2));

        LoadPage_1.onClick.AddListener(() => switch_page(LoadPage_saveload1, LoadPage_saveload2, LoadPage_saveload3));
        LoadPage_2.onClick.AddListener(() => switch_page(LoadPage_saveload2, LoadPage_saveload1, LoadPage_saveload3));
        LoadPage_3.onClick.AddListener(() => switch_page(LoadPage_saveload3, LoadPage_saveload1, LoadPage_saveload2));
        
    }

    public void switch_page(GameObject to_show, GameObject to_hide1, GameObject to_hide2)
    {
        to_show.SetActive(true);
        to_hide1.SetActive(false);
        to_hide2.SetActive(false);
    }
}
