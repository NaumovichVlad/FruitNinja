using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Image darkEffect;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text bestScoreLabel;
    [SerializeField] private string nextSceneName;
    [SerializeField] private string bestScoreKey;
    [SerializeField] private float darkEffectSpeed;

    private bool _isStartbuttonClicked;

    private void Awake()
    {
        startButton.onClick.AddListener(StartButtonClick);
        exitButton.onClick.AddListener(ExitButtonClick);

        var score = 0;

        if (PlayerPrefs.HasKey(bestScoreKey))
        {
            score = PlayerPrefs.GetInt(bestScoreKey);
        }

        bestScoreLabel.text += score;
    }

    private void Update()
    {
        if(_isStartbuttonClicked)
        {
            var color = darkEffect.color;

            if (darkEffect.color.a < 1)
            {
                color.a += darkEffectSpeed;
                darkEffect.color = color;
            }
            else
            {
                SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
            }
        }
    }

    private void StartButtonClick()
    {
        _isStartbuttonClicked = true;
        exitButton.enabled = false;
        startButton.enabled = false;
    }

    private void ExitButtonClick()
    {
        Application.Quit();
#if DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
