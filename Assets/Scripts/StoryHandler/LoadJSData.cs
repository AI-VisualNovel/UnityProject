using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Theme
{
    public string themeName;
    public List<Story> stories;
}

[System.Serializable]
public class Story
{
    public string storyName;
    public Dictionary<string, Block> blocks;
}

[System.Serializable]
public class Block
{
    public string plot;
    public Dictionary<string, string> options;
}

[System.Serializable]
public class StoryData
{
    public List<Theme> themes;
}

public class LoadJSData : MonoBehaviour
{

    // StoryData data = LoadStoryData();
    // 0->鬼故事 ; 1->武俠 ; 2->XX
    // public void OnButtonClick()
    // {
    //     // 載入資料

    //     // 示範: 修改資料
    //     Debug.Log(data["key2"]);
    // }


    void Start()
    {
        StoryData data = LoadStoryData();

        // 取出第一個主題的名稱
        Theme firstTheme = data.themes[0];
        Debug.Log("第一個主題名稱: " + firstTheme.themeName);

        // 取出第一個主題下的第一個故事名稱
        Story firstStory = firstTheme.stories[0];
        Debug.Log("第一個故事名稱: " + firstStory.storyName);

        // 取出第一個故事的第一個區塊的情節
        Block firstBlock = firstStory.blocks["block1"];
        Debug.Log("第一個情節描述: " + firstBlock.plot);

        // 取出第一個選項 (我們從options字典中取得第一個key和相對應的值)
        var firstOptionKey = firstBlock.options.Keys.GetEnumerator();
        firstOptionKey.MoveNext();
        string optionKey = firstOptionKey.Current;
        string optionValue = firstBlock.options[optionKey];
        Debug.Log("第一個選項: " + optionKey + " -> " + optionValue);
    }

    // Update is called once per frame
    StoryData LoadStoryData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("Story/data");
        return JsonConvert.DeserializeObject<StoryData>(jsonData.text);
    }

}
