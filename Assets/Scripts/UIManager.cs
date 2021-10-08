using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    private Button _continue;
    private Button _pause;
    private readonly List<Button> _toMain = new List<Button>();
    private TextMeshProUGUI _scoreTotal, _pairsTotal;

    private void Awake()
    {
        SetHash();
        SetListeners();
    }

    private void SetHash()
    {
        _scoreText = GameObject.Find("Canvas/Header/Score").GetComponent<TextMeshProUGUI>();
        GameManager.Gm.onScoreUpdate.AddListener(UpdateScoreText);
        
        _pause = GameObject.Find("Pause").GetComponent<Button>();
        _continue = GameObject.Find("Continue").GetComponent<Button>();
        _toMain.Add(GameObject.Find("ResultPanel/ToMain").GetComponent<Button>());
        _toMain.Add(GameObject.Find("PauseMenu/ToMain").GetComponent<Button>());
        _pairsTotal = GameObject.Find("ResultPanel/TotalPairs").GetComponent<TextMeshProUGUI>();
        _scoreTotal = GameObject.Find("ResultPanel/TotalScore").GetComponent<TextMeshProUGUI>();
        
        _continue.transform.parent.gameObject.SetActive(false);
        _scoreTotal.transform.parent.gameObject.SetActive(false);
    }
    private void SetListeners()
    {
        GameManager.Gm.onAllPairsMatched.AddListener(() =>
        {
            _scoreTotal.transform.parent.gameObject.SetActive(true);
            _scoreTotal.text = $"Total Score: {GameManager.Gm.Score.ToString()}";
            _pairsTotal.text = $"Total matched pairs: {GameManager.Gm._pairs.ToString()}";
        });
        _pause.onClick.AddListener(() => 
        { 
            Time.timeScale = 0;
            GameManager.Gm.GameState = GameManager.gameState.pause;
        });
        _continue.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            GameManager.Gm.GameState = GameManager.gameState.play;
        });
        foreach (var item in _toMain)
        {
            item.onClick.AddListener(() => { SceneManager.LoadScene("Main"); });
        }
    }
    private void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
    
}
