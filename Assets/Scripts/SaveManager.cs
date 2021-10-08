using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private static string _path;
    private static SavePattern _savePattern = new SavePattern();
    void Awake()
    {
#if UNITY_EDITOR
        _path = Path.Combine(Application.dataPath, "save.json");
#else
        _path = Path.Combine(Application.persistentDataPath, "save.json");
#endif
    }
    public static void LoadSave(ref int score, ref int pairs)
    {
        if(!File.Exists(_path)) return;
        
        _savePattern = JsonUtility.FromJson<SavePattern>(File.ReadAllText(_path));
        score = _savePattern.totalScore;
        pairs = _savePattern.totalPairs;
    }
    public static void CreateOrRewriteSave(ref int score, ref int pairs)
    {
        _savePattern.totalScore = score;
        _savePattern.totalPairs = pairs;
        File.WriteAllText(_path, JsonUtility.ToJson(_savePattern));
    }
}

public class SavePattern
{
    public int totalScore;
    public int totalPairs;
}
