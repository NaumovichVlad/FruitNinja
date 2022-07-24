using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopUpController : MonoBehaviour
{
    public static event OnLose LoseEvent;
    public static event OnLose RestartEvent;
    public delegate void OnLose();

    private int _score;

    [SerializeField] private Text loseScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image background;
    [SerializeField] private float maxBackgroundTransparency;
    [SerializeField] private float transparencySpeed;

    void Start()
    {
        _score = ScoreCounterController.GetInstance().GetScore();
        loseScoreText.text += _score;
        LoseEvent();

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
    }

    private void RestartButtonClick()
    {
        RestartEvent();
        Destroy(gameObject);
    }

    private void MainMenuButtonClick()
    {
        Application.Quit();
#if DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
