using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreKuis : MonoBehaviour
{
    public string[] shortMessageData;

    public TextMeshProUGUI textShortMessage;
    public TextMeshProUGUI textPoints;

    public TextMeshProUGUI textCorrectAmount;
    public TextMeshProUGUI textWrongAmount;


    public event Action OnDone;

    public void InitializeScore(int correctAnswer)
    {
        string scoreMessage = "";

        if (correctAnswer >= 10)
            scoreMessage = shortMessageData[2];
        else if (correctAnswer <= 9 && correctAnswer >= 1)
            scoreMessage = shortMessageData[1];
        else
            scoreMessage = shortMessageData[0];

        textShortMessage.text = scoreMessage;
        textPoints.text = $"{correctAnswer * 10}";

        textCorrectAmount.text = $"{correctAnswer} Soal";
        textWrongAmount.text = $"{10 - correctAnswer} Soal";
    }

    public void Done()
    {
        OnDone?.Invoke();
    }
}