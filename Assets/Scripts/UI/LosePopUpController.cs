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

    void Start()
    {
        _score = ScoreCounterController.GetInstance().GetScore();
        loseScoreText.text += _score;
        LoseEvent();
        restartButton.onClick.AddListener(RestartButtonClick);
        mainMenuButton.onClick.AddListener(MainMenuButtonClick);
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
