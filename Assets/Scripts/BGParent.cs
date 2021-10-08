
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGParent : MonoBehaviour
{
    private static BGParent _bgParent;
    private List<Background> _backgrounds;

    private void Start()
    {
        if (_bgParent == null)
        {
            _bgParent = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _backgrounds = GetComponentsInChildren<Background>().ToList();
        SceneManager.sceneLoaded += ManageBG;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ManageBG;
    }

    private void ManageBG(Scene s, LoadSceneMode m)
    {
        if (s == SceneManager.GetSceneByName("SampleScene"))
        {
            foreach (var item in _backgrounds)
            {
                item.enabled = true;
            }
        }else if (s == SceneManager.GetSceneByName("Main"))
        {
            foreach (var item in _backgrounds)
            {
                item.enabled = false;
            }
        }
    }
}
