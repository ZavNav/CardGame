using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private static string _path;
    private static SavePattern _savePattern = new SavePattern();

    private void Awake()
    {
        ChooseLocation(PlayerPrefs.GetString("gameMode"));
    }

    private void ChooseLocation(string fileName)
    {
#if UNITY_EDITOR
        _path = Path.Combine(Application.dataPath, $"{fileName}.json");
#else
        _path = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
#endif
    }
    
    public static void LoadSave(ref int score, ref int pairs, ref int level)
    {
        if(!File.Exists(_path)) return;
        
        _savePattern = JsonUtility.FromJson<SavePattern>(File.ReadAllText(_path));
        score = _savePattern.totalScore;
        pairs = _savePattern.totalPairs;
        level = _savePattern.level;
    }
    public static void CreateOrRewriteSave(ref int score, ref int pairs, ref int level)
    {
        _savePattern.totalScore = score;
        _savePattern.totalPairs = pairs;
        _savePattern.level = level;
        File.WriteAllText(_path, JsonUtility.ToJson(_savePattern));
    }

    public static void RemoveSaves()
    {
        _savePattern.totalScore = 0;
        _savePattern.totalPairs = 0;
        _savePattern.level = 1;
        File.WriteAllText(_path, JsonUtility.ToJson(_savePattern));
    }
}

public class SavePattern
{
    public int totalScore;
    public int totalPairs;
    public int level;
}
