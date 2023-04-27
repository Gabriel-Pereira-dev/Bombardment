using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    // Constants
    private static readonly string KEY_HIGHEST_SCORE = "HighestScore";

    // API

    public bool isGameOver { get; private set; }
    [Header("Audio")]

    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource gameOverSfx;

    [Header("Score")]

    [SerializeField] private float score;
    [SerializeField] private int highestScore;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Score
        score = 0f;
        highestScore = PlayerPrefs.GetInt(KEY_HIGHEST_SCORE); ;
    }

    void Update()
    {
        if (!isGameOver)
        {
            score += Time.deltaTime;

            if (GetScore() > GetHighestScore())
            {
                highestScore = GetScore();
            }
        }

    }

    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }

    public int GetHighestScore()
    {
        return highestScore;
    }

    public void EndGame()
    {
        if (isGameOver) return;

        // Set flag
        isGameOver = true;

        // Stop Music
        musicPlayer.Stop();

        // Play SFX
        gameOverSfx.Play();

        // SaveHighestScore
        PlayerPrefs.SetInt(KEY_HIGHEST_SCORE, GetHighestScore());

        // Reload Scene
        StartCoroutine(ReloadScene(5f));
    }

    private IEnumerator ReloadScene(float delay)
    {
        // Wait
        yield return new WaitForSeconds(delay);

        // Reload Scene
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }




}
