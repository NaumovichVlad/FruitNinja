using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePopUpController : MonoBehaviour
{
    public static event OnLose RestartEvent;
    public delegate void OnLose();

    private int _score;
    private bool _isMainMenuButtonClicked;

    [SerializeField] private Text loseScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image background;
    [SerializeField] private Image darkEffect;
    [SerializeField] private float maxBackgroundTransparency;
    [SerializeField] private float transparencySpeed;
    [SerializeField] private string mainSceneName;
    [SerializeField] private float darkEffectSpeed;

    void Start()
    {
        _score = ScoreCounterController.GetInstance().GetScore();
        loseScoreText.text += _score;

        restartButton.onClick.AddListener(RestartButtonClick);
        mainMenuButton.onClick.AddListener(MainMenuButtonClick);

        var color = background.color;
        color.a = 0;
        background.color = color;
    }

    private void Update()
    {
        var color = background.color;

        if (color.a < maxBackgroundTransparency)
        {
            color.a += transparencySpeed * Time.deltaTime;
            background.color = color;
        }

        if (_isMainMenuButtonClicked)
        {
            var darkColor = darkEffect.color;

            if (darkEffect.color.a < 1)
            {
                darkColor.a += darkEffectSpeed;
                darkEffect.color = color;
            }
            else
            {
                SceneManager.LoadScene(mainSceneName, LoadSceneMode.Single);
            }
        }
    }

    private void RestartButtonClick()
    {
        RestartEvent();
        Destroy(gameObject);
    }

    private void MainMenuButtonClick()
    {
        _isMainMenuButtonClicked = true;
        restartButton.enabled = false;
        restartButton.enabled = false;
    }
}
