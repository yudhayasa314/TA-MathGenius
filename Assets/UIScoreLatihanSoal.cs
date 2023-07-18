using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreLatihanSoal : MonoBehaviour
{
    [System.Serializable]
    public class ScoreMessage
    {
        [TextArea]
        public string shortMessage;

        [TextArea]
        public string fullMessage;
    }

    public ScoreMessage[] scoreMessages;

    public UIStar uiStar;
    public TextMeshProUGUI textShortMessage;
    public TextMeshProUGUI textMessage;

    public event Action OnRetry;
    public event Action OnDone;

    public void InitializeScore(int correctAnswer)
    {
        int starCollected = 0;

        #region Start Calculation
        if (correctAnswer >= 5)
            starCollected = 3;
        else if (correctAnswer >= 3 && correctAnswer <= 4)
            starCollected = 2;
        else if (correctAnswer >= 1 && correctAnswer <= 2)
            starCollected = 1;
        else
            starCollected = 0;
        #endregion

        uiStar.SetStar(starCollected);
        textShortMessage.text = scoreMessages[starCollected].shortMessage;
        textMessage.text = scoreMessages[starCollected].fullMessage;
    }

    public void Done()
    {
        OnDone?.Invoke();
    }

    public void Retry()
    {
        OnRetry?.Invoke();
    }
}
