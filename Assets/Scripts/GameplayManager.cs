using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    [Header("Time")]
    public float timeToReduceCoolDown = 30.0f;
    private float elapsedTime;

    [Header("Canvas")]
    public Image crosshair;
    public GameObject bulletInfo;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScoreText;

    [Header("GameOverCanvas")]
    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverHighScoreText;

    public int score;
    public int highScore; 

    private Enemy2 enemy;


    void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();

        crosshair.gameObject.SetActive(true);

        gameOverUI.SetActive(false);
        bulletInfo.SetActive(true);

        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeToReduceCoolDown)
        {
            FindObjectOfType<EnemySpawn>().ReduceCoolDown();
            elapsedTime = 0f;
        }
    }


    public void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    public void EnemyDied()
    {
        score++;
        UpdateHighScore();
        UpdateUI();
    }

    public void UpdateUI()
    {
        ScoreText.text = "Score: " + score.ToString();
        gameOverScoreText.text = "Score: " + score.ToString();

        HighScoreText.text = "High Score: " + highScore.ToString();
        gameOverHighScoreText.text = "High Score: " + highScore.ToString();
    }


    public void GameOver()
    {
        crosshair.gameObject.SetActive(false);
        bulletInfo.SetActive(false);

        gameOverUI.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        bulletInfo.SetActive(true);

        crosshair.gameObject.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
