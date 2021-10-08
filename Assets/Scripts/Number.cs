using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Number : MonoBehaviour, IPointerDownHandler
{
    public int num;
    private Vector3 _target;
    [SerializeField] private float speed;

    private Image _outline;
    void Start()
    {
        SetHash();
    }
    private void SetHash()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = num.ToString();
        _target = GameManager.Gm.GetRandomPosInView();
        
        GetComponent<Image>().color = GetRandomColor();
        _outline = transform.GetChild(1).GetComponent<Image>();
        GameManager.Gm.onPairChecked.AddListener(RemoveOutline);
    }
    
    void Update()
    {
        NumberMovement();
    }
    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    private void NumberMovement()
    {
        if (_target == transform.position)
        {
            _target = GameManager.Gm.GetRandomPosInView();
        }
        else
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameManager.Gm.GameState == GameManager.gameState.pause) return;
        
        _outline.color = new Color(0, 0, 0, 1);
        GameManager.Gm.SetChoise(this);
    }
    private void RemoveOutline()
    {
        _outline.color = new Color(0, 0, 0, 0);
    }
}
