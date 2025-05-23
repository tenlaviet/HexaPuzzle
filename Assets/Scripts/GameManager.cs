using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    
    private int _score;
    private float _elapsedTime;
    
    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateTimer();
        UpdateScore(_score);
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }


    private void Update()
    {
        UpdateTimer();
    }
    
    private void UpdateTimer()
    {
        _elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        m_timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateScore(int number)
    {
        _score += number;
        m_scoreText.text = _score.ToString();
    }
    // public void IncreaseScore(int points)
    // {
    //     SetScore(score + points);
    // }
    //
    // private void SetScore(int score)
    // {
    //     this.score = score;
    //     scoreText.text = score.ToString();
    // }
}
