using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounterController : MonoBehaviour
{
    private int _score = 0;
    private static ScoreCounterController instance;

    [SerializeField] private Text scoreText;

    void Awake()
    {
        instance = this;
    }

    public static ScoreCounterController GetInstance()
    {
        return instance;
    }

    public int GetScore()
    {
        return _score;
    }

    public void AddScore()
    {
        _score++;
        scoreText.text = _score.ToString();
    }


    void Update()
    {
        
    }
}
