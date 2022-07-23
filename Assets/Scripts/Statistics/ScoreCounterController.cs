using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounterController : MonoBehaviour
{
    private int _score = 0;
    private int _combo = 1;
    private float _lastCutTime = 0;
    private int _bestScore = 0;

    private static ScoreCounterController instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private ComboTextController comboTextController;
    [SerializeField] private string bestScoreKey;
    [SerializeField] private int maxCombo;
    [SerializeField] private float maxTimeForCombo;

    void Awake()
    {
        instance = this;

        InitializeBestScore();
    }

    public static ScoreCounterController GetInstance()
    {
        return instance;
    }

    public int GetScore()
    {
        return _score;
    }

    public void AddScore(GameObject cuttedObject)
    {
        var time = Time.realtimeSinceStartup;

        if (time - _lastCutTime < maxTimeForCombo)
        {
            if (_combo < maxCombo)
            {
                _combo++;
            }

            comboTextController.SetCombo(_combo);

            var comboLabel = Instantiate(comboTextController.gameObject, gameObject.transform.parent);

            comboLabel.transform.position = cuttedObject.transform.position;
        }
        else
        {
            _combo = 1;
        }

        _score += _combo;
        scoreText.text = _score.ToString();
        _lastCutTime = time;

        if (_bestScore < _score)
        {
            _bestScore = _score;
            SetBestScoreText();
        }
    }
    
    private void InitializeBestScore()
    {
        if (PlayerPrefs.HasKey(bestScoreKey))
        {
            _bestScore = PlayerPrefs.GetInt(bestScoreKey);
            SetBestScoreText();
        }
        else
        {
            PlayerPrefs.SetInt(bestScoreKey, _bestScore);
        }
    }

    private void SetBestScoreText()
    {
        bestScoreText.text = string.Format("Best: {0}", _bestScore);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(bestScoreKey, _bestScore);
    }
}
