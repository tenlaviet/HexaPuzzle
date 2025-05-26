using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private GameObject m_pauseWindow;
    [HideInInspector]public HexBoard Board;
    private int _score;
    private float _elapsedTime;
    public bool IsPaused { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateTimer();
        UpdateScore(_score);
    }
    
    private void Update()
    {
        UpdateTimer();
    }
    
    private void UpdateTimer()
    {
        if (IsPaused)
        {
            return;
        }
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

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        m_pauseWindow.SetActive(IsPaused);
    }

    public void Restart()
    {
        _score = 0;
        _elapsedTime = 0;
        UpdateTimer();
        UpdateScore(0);
        Board.RestartBoard();
        TogglePause();
    }
}
