using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerProgression : MonoBehaviour
{

    ProgressionManager progressionManager;

    [System.Serializable]
    public class MessageData
    {
        public string shortMessage;
        public string fullMessage;
    }

    public MessageData[] dataMessage;

    [Header("Star")]

    public TextMeshProUGUI textStarAmount;
    public TextMeshProUGUI textStarShortMessage;
    public TextMeshProUGUI textStarFullMessage;


    [Header("Score")]

    public TextMeshProUGUI textScoreAmount;
    public TextMeshProUGUI textScoreShortMessage;
    public TextMeshProUGUI textScoreFullMessage;


    // Start is called before the first frame update
    void Start()
    {
        progressionManager = ProgressionManager.instance;

        SetAllProgress();
    }

    void SetAllProgress()
    {
        int totalBintang = progressionManager.GetTotalScoreLatihanSoal;
        textStarAmount.text = $"{totalBintang}";

        if (totalBintang > 3)
        {
            textStarShortMessage.text = dataMessage[1].shortMessage;
            textStarFullMessage.text = $"Kamu Sudah Meraih {totalBintang} Bintang Sejauh Ini. Jangan Cepat Puas Dan Terus Belajar Ya..";

        }
        else
        {
            textStarShortMessage.text = dataMessage[0].shortMessage;
            textStarFullMessage.text = dataMessage[0].fullMessage;
        }

        int totalScore = progressionManager.GetTotalScoreKuis;
        textScoreAmount.text = $"{totalScore}";

        if(totalScore > 100)
        {
            textScoreShortMessage.text = dataMessage[1].shortMessage;
            textScoreFullMessage.text = $"Kamu Sudah Meraih Skor {totalScore} Sejauh Ini. Jangan Cepat Puas Dan Terus Belajar Ya..";
        }
        else
        {
            textScoreShortMessage.text = dataMessage[0].shortMessage;
            textScoreFullMessage.text = dataMessage[0].fullMessage;
        }
    }

    
}
