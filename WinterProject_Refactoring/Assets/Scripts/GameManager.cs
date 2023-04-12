using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public SaveData saveData;

    private string path;
    private new void Awake()
    {
        path = Path.Combine(Application.persistentDataPath, "save.json");
        LoadData();
    }

    void LoadData()
    {
        if (!File.Exists(path)) return;

        saveData = new SaveData();
        var jsonData = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(jsonData);
    }

    public void SaveData()
    {
        var jsonData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, jsonData);
    }
}
