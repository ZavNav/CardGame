using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float _horizontalSize = 19.15f;
    private float speed;
    public enum bgType
    {
        cloud,
        ground_1,
        ground_2,
        ground_3,
    }

    public bgType BgType;
    private static GameObject _bgParent;
    
    void Start()
    {
       // float cameraSize = GameManager.Gm.GetCameraVerticalSize();
       // transform.localScale = new Vector3(cameraSize / 10, cameraSize / 10, 1);
        switch (BgType)
        {
            case bgType.cloud:
                speed = 1;
                break;
            case bgType.ground_1:
                speed = 2;
                break;
            case bgType.ground_2:
                speed = 3;
                break;
            case bgType.ground_3:
                speed = 4;
                break;
        }
    }

    private void Update()
    {
        transform.Translate(-speed*Time.deltaTime/10, 0, 0);
    }

    private void LateUpdate()
    {
        if (transform.position.x + _horizontalSize/2 < GameManager.Gm.GetCameraLeftBorder())
        {
            transform.Translate(_horizontalSize*2, 0, 0);
        }
    }
}
