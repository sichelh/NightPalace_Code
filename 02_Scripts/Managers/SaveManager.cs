using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string defaultJsonPath = "Assets/Data/";

    public void SaveJson<T>(T data, string fileName) // -> ToJson -> Write
    {
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log($"{data} 저장하엿습니다.");
        Debug.Log($"{jsonData} 저장하엿습니다.");
        string jsonPath = $"{defaultJsonPath}{fileName}.json";

        File.WriteAllText(jsonPath, jsonData);
    }

    public bool TryLoadJson<T>(out T data, string fileName) // Read -> FromJson
    {
        string jsonPath = $"{defaultJsonPath}{fileName}.json";
        if (File.Exists(jsonPath))
        {
            string jsonData = File.ReadAllText(jsonPath);
            data = JsonUtility.FromJson<T>(jsonData);
            return true;
        }
        data = default;
        return false;
    }
}
