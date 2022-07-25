using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text bestScoreLabel;
    [SerializeField] private string nextSceneName;
    [SerializeField] private string bestScoreKey;

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

    private void StartButtonClick()
    {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    private void ExitButtonClick()
    {
        Application.Quit();
#if DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
