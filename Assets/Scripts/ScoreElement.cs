using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI usernameText;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void NewScoreElement(string _username, string _score)
    {
            usernameText.text = string.Format(_username);
            scoreText.text = string.Format(_score);
    }
}
