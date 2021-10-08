using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public  class OnScoreUpdate : UnityEvent<int>{}
public class GameManager : MonoBehaviour
{
    public static GameManager Gm;
    
    public enum gameState
    {
        play,
        pause
    }

    public gameState GameState;
    
    private Camera _cam;
    private Transform _canvasTransform;
    private Number _numberPref;

    private int _maxNumbers = 10;
    private int _numbersCounter = 0;

    private readonly List<int> _allNumbers = new List<int>();
    [SerializeField]private List<Number> _pair = new List<Number>();
    public readonly OnScoreUpdate onScoreUpdate = new OnScoreUpdate();
    public UnityEvent onAllPairsMatched = new UnityEvent();

    public UnityEvent onPairChecked = new UnityEvent();
    public int Score;
    public int _pairs;
    
    void Awake()
    {
        GameState = gameState.play;
        SetHash();
    }

    private void Start()
    {
        SaveManager.LoadSave(ref Score, ref _pairs);
        onScoreUpdate?.Invoke(Score);
        StartSpawn();
    }

    private void OnApplicationQuit()
    {
        SaveManager.CreateOrRewriteSave(ref Score, ref _pairs);
    }

    private void SetHash()
    {
        Gm = this;
        _cam = Camera.main;
        _canvasTransform = FindObjectOfType<Canvas>().transform;
        _numberPref = Resources.Load<Number>("NumberPref");
    }
    public float GetCameraVerticalSize()
    {
        return _cam.orthographicSize * 2;
    }

    public float GetCameraLeftBorder()
    {
        return _cam.aspect * _cam.orthographicSize * -1;
    }
    private void StartSpawn()
    {
        for (int i = 0; i < _maxNumbers; i++)
        {
            SpawnNumber();
        }
        Correction();
    }
    private void SpawnNumber(int count = 0)
    {
        GameObject temp = 
            Instantiate(_numberPref.gameObject, GetRandomPosInView(), 
                Quaternion.identity, _canvasTransform.GetChild(0));
        _numbersCounter++;
        temp.name = $"Number_{_numbersCounter}";
        if (count == 0) count = Random.Range(1, 10);
        _allNumbers.Add(temp.GetComponent<Number>().num = count);
    }
    public Vector3 GetRandomPosInView()
    {
        float yCamSize = _cam.orthographicSize;
        float xCamSize = _cam.aspect * yCamSize;
        
        return new Vector3(Random.Range(-xCamSize, xCamSize), Random.Range(-yCamSize+1, yCamSize-1), 0);
    }
    private void Correction()
    {
        if (_allNumbers.Sum() % 10 != 0)
        {
            SpawnNumber(_allNumbers.Sum() % 10);
        }
        var numDict = _allNumbers
            .GroupBy(x => x)
            .Select(g => new {num = g.Key, count = g.Count()});
        foreach (var item in numDict)
        {
            if (item.count % 2 != 0)
            {
                SpawnNumber(item.num);
            }
        }
    }
    public void SetChoise(Number number)
    {
        _pair.Add(number);
        if (_pair.Count == 2)
        {
            CheckPair();
        }
    }
    private void CheckPair()
    {
        bool nums = _pair[0].num == _pair[1].num;
        bool objs = _pair[0] != _pair[1];
        bool sum = _pair[0].num + _pair[1].num == 10;
        if ((nums || sum) && objs)
        {
            Score += 10;
            _pairs++;
            _numbersCounter-=2;
            onScoreUpdate?.Invoke(Score);
            foreach (var item in _pair)
            {
                Destroy(item.gameObject);
            }

            if (_numbersCounter == 0)
            {
                onAllPairsMatched?.Invoke();
                SaveManager.CreateOrRewriteSave(ref Score, ref _pairs);
            }
        }
        _pair.Clear();
        onPairChecked?.Invoke();
    }

    
}
