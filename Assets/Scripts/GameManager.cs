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

    
    private int score = 0;
    private float elapsedTime;
    
    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
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
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        m_timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
