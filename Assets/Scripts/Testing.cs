using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class Testing : MonoBehaviour
{
   public void Test(){
        // 把位置也包到Json
        // UpdateJsonLocation(button);
        string json = File.ReadAllText(Application.streamingAssetsPath + "/Json/08-22-23-16-09-46.json");

        // 將JSON解析為C#對象
        GameData data = JsonConvert.DeserializeObject<GameData>(json);
        data.Location = "test";
        json = JsonUtility.ToJson(data,true);
        File.WriteAllText(Application.streamingAssetsPath + "/Json/08-22-23-16-09-46.json", json);


        string json2 = File.ReadAllText(Application.streamingAssetsPath + "/Json/08-22-23-16-09-46.json");
        GameData data2 = JsonUtility.FromJson<GameData>(json2);
        Debug.Log(data2.Location);
   }
}
