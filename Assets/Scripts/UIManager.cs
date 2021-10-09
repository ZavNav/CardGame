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
    private Button _next;
    private readonly List<Button> _toMain = new List<Button>();
    private TextMeshProUGUI _scoreTotal, _pairsTotal;

    private TextMeshProUGUI _timer;
    private int _seconds = 59, _minutes = 1;

    private void Awake()
    {
        SetHash();
        SetListeners();
        if (PlayerPrefs.GetString("gameMode") != "save") StartCoroutine(Timer());
    }

    private void SetHash()
    {
        _scoreText = GameObject.Find("Canvas/Header/Score").GetComponent<TextMeshProUGUI>();
        GameManager.Gm.onScoreUpdate.AddListener(UpdateScoreText);
        
        _pause = GameObject.Find("Pause").GetComponent<Button>();
        _continue = GameObject.Find("Continue").GetComponent<Button>();
        _next = GameObject.Find("Next").GetComponent<Button>();
        _toMain.Add(GameObject.Find("ResultPanel/ToMain").GetComponent<Button>());
        _toMain.Add(GameObject.Find("PauseMenu/ToMain").GetComponent<Button>());
        _pairsTotal = GameObject.Find("ResultPanel/TotalPairs").GetComponent<TextMeshProUGUI>();
        _scoreTotal = GameObject.Find("ResultPanel/TotalScore").GetComponent<TextMeshProUGUI>();

        _timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        _timer.gameObject.SetActive(PlayerPrefs.GetString("gameMode") != "save");
            
        _continue.transform.parent.gameObject.SetActive(false);
        _scoreTotal.transform.parent.gameObject.SetActive(false);
    }

    private IEnumerator Timer()
    {
        while (_timer.text != "00:00")
        {
            if (_seconds == 0)
            {
                _minutes--;
                yield return new WaitForSeconds(1);
                _seconds = 59;
                _timer.text = _minutes.ToString("00") + ":" + _seconds.ToString("00");
            }
            yield return new WaitForSeconds(1);
            _seconds--;
            _timer.text = _minutes.ToString("00") + ":" + _seconds.ToString("00");
        }
        TimeOver();
    }

    private void TimeOver()
    {
        SaveManager.RemoveSaves();
        Time.timeScale = 0;
        var resultPanel = _scoreTotal.transform.parent.gameObject;
        resultPanel.SetActive(true);
        resultPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "YOU LOSE";
        resultPanel.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = "RESTART";
        _scoreTotal.text = $"Total Score: {GameManager.Gm.Score.ToString()}";
        _pairsTotal.text = $"Total matched pairs: {GameManager.Gm.pairs.ToString()}";
        GameManager.Gm.Score = 0;
        GameManager.Gm.pairs = 0;
    }
    private void SetListeners()
    {
        GameManager.Gm.onAllPairsMatched.AddListener(() =>
        {
            Time.timeScale = 0;
            _scoreTotal.transform.parent.gameObject.SetActive(true);
            _scoreTotal.text = $"Total Score: {GameManager.Gm.Score.ToString()}";
            _pairsTotal.text = $"Total matched pairs: {GameManager.Gm.pairs.ToString()}";
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
        _next.onClick.AddListener(() =>
        {
            SaveManager.CreateOrRewriteSave(ref GameManager.Gm.Score, ref GameManager.Gm.pairs,
                ref GameManager.Gm.level);
            SceneManager.LoadScene("SampleScene");
        });
        foreach (var item in _toMain)
        {
            item.onClick.AddListener(() =>
            {
                SaveManager.CreateOrRewriteSave(ref GameManager.Gm.Score, ref GameManager.Gm.pairs,
                    ref GameManager.Gm.level);
                SceneManager.LoadScene("Main"); 
            });
        }
    }
    private void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
    
}
