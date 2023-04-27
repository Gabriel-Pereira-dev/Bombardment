using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUIScript : MonoBehaviour
{
    private static readonly int SCORE_FACTOR = 10;

    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI highestScoreLabel;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUITexts();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUITexts();
    }

    void UpdateUITexts()
    {
        scoreLabel.text = GetScoreString();
        highestScoreLabel.text = GetHighhestScoreString();
    }

    private string GetScoreString()
    {
        return (GameManager.Instance.GetScore() * SCORE_FACTOR).ToString();
    }
    private string GetHighhestScoreString()
    {
        return (GameManager.Instance.GetHighestScore() * SCORE_FACTOR).ToString();
    }


}
