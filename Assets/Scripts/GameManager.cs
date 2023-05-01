using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    // Constants
    private static readonly string KEY_HIGHEST_SCORE = "HighestScore";
    [HideInInspector] public static readonly string KEY_SELECTED_CHARACTER_INDEX = "SelectedCharacterIndex";

    // API

    public bool isGameOver { get; private set; }

    [Header("Audio")]

    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource gameOverSfx;

    [Header("Score")]

    [SerializeField] private float score;
    [SerializeField] private int highestScore;

    [Header("Character")]
    public int selectedCharacterIndex;
    public GameObject[] characters;
    public string characterName;
    public new CinemachineVirtualCamera camera;

    [Header("Cannon")]

    public List<GameObject> cannons;



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
        highestScore = PlayerPrefs.GetInt(KEY_HIGHEST_SCORE);

        // Character
        selectedCharacterIndex = PlayerPrefs.GetInt(KEY_SELECTED_CHARACTER_INDEX);
        LoadCharacter();
    }

    void Start()
    {
        LoadCharacter();
        StartCoroutine(EnableNextCannonCoroutine());
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

    private void LoadCharacter()
    {
        GameObject prefab = characters[selectedCharacterIndex];
        prefab.SetActive(true);
        camera.m_Follow = prefab.transform;
        camera.m_LookAt = prefab.transform;

        characterName = prefab.name;

    }

    private List<GameObject> GetDisabledCannons()
    {
        List<GameObject> disabledsCannons = new List<GameObject>();
        foreach (GameObject c in cannons)
        {
            if (!c.activeSelf)
            {
                disabledsCannons.Add(c);
            }
        }
        return disabledsCannons;
    }

    private IEnumerator EnableNextCannonCoroutine()
    {
        List<GameObject> currentDisabledsCannons = GetDisabledCannons();
        int disabledsCannonsQuantity = currentDisabledsCannons.Count;
        for (int i = 0; i < disabledsCannonsQuantity; i++)
        {
            yield return new WaitForSeconds(30f);
            currentDisabledsCannons[Random.Range(0, currentDisabledsCannons.Count)].SetActive(true);
            currentDisabledsCannons = GetDisabledCannons();
        }
    }


}
