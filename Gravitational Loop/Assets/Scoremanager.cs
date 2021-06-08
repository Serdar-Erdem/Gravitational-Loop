﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager intance;

    public int score;

    private Text scoreText;

    private void Awake()
    {

        makeSingleton();
        scoreText = GameObject.Find("scoresText").GetComponent<Text>();
    }

    private void makeSingleton()
    {
        if (intance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            intance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        addScore(0);
    }

    void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("scoresText").GetComponent<Text>();
        }
    }

    public void addScore(int value)
    {
        score += value;
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
