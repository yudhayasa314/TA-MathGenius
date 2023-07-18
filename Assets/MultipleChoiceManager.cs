using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MultipleChoiceManager : MonoBehaviour
{
    ProgressionManager progressionManager;

    public GameObject framePlaceholderJawaban;
    public GameObject frameJawaban;
    public TextMeshProUGUI textFrameJawaban;

    public TextMeshProUGUI textSoal;
    public UIButtonMultipleChoice[] btnMultipleChoices;

    public DOTweenAnimation vfxIncorrect;

    public List<LatihanSoalSO> soalDatabaseLatihanSoal = new List<LatihanSoalSO>();
    public List<LatihanSoalSO> soalDatabaseKuis = new List<LatihanSoalSO>();

    public List<LatihanSoalSO> soalData = new List<LatihanSoalSO>();

    public MateriBelajar uiPembahasan;
    public GameObject coverUnTouch;
    public UIScoreLatihanSoal UIScoreLatihanSoal;
    public UIScoreKuis UIScoreKuis;

    [Header("Status")]
    public int currentSoal;
    public int correctAnswerTotal;
    bool isAnswered;
    bool isLatihanSoal;
    int currentTypeSoal = 99;

    public event Action OnCorrect;
    public event Action OnInCorrect;

    public UnityEvent OnCorrectEvent;
    public UnityEvent OnWrongEvent;
    public UnityEvent OnGetAnyStar;
    public UnityEvent OnLoseAllStar;

    // Start is called before the first frame update
    void Start()
    {
        progressionManager = ProgressionManager.instance;

        coverUnTouch.gameObject.SetActive(false);

        currentTypeSoal = 99;

        for (int i = 0; i < btnMultipleChoices.Length; i++)
        {
            btnMultipleChoices[i].OnAnswered += MultipleChoiceManager_OnAnswered;
        }

        GenerateSoal();
        SetSoal();
    }

    void GenerateSoal()
    {
        isLatihanSoal = progressionManager.typeSoal == TypeSoal.LATIHAN_SOAL;
        int maxRange = 4;
        int soalPerType = 5;

        if (isLatihanSoal)
            soalPerType = 5;
        else
            soalPerType = 10;

        maxRange = soalDatabaseLatihanSoal.Count / soalPerType;

        if (currentTypeSoal == 99)
        {
            currentTypeSoal = Random.Range(0, maxRange);
        }
        else
        {
            int tempTypeSoal = currentTypeSoal;

            while (currentTypeSoal == tempTypeSoal)
            {
                currentTypeSoal = Random.Range(0, maxRange);
            }
        }


        int addition = currentTypeSoal * soalPerType;

        soalData.Clear();

        for (int i = 0; i < soalPerType; i++)
        {
            if (isLatihanSoal)
            {
                soalData.Add(soalDatabaseLatihanSoal[i + addition]);
                uiPembahasan.content[i].audioTextToSpeech = soalDatabaseLatihanSoal[i + addition].clipPembahasan;
                uiPembahasan.content[i].content.transform.GetChild(2).GetComponent<Image>().sprite = soalDatabaseLatihanSoal[i + addition].pembahasanPict;
            }   
            else
                soalData.Add(soalDatabaseKuis[i + addition]);

        }
    }

    private void MultipleChoiceManager_OnAnswered(bool obj)
    {
        if (isAnswered)
            return;

        isAnswered = true;

        if (obj)
        {
            correctAnswerTotal++;
            framePlaceholderJawaban.gameObject.SetActive(false);
            frameJawaban.gameObject.SetActive(true);

            Debug.Log("JAWABAN BENAR");
            OnCorrect?.Invoke();
            OnCorrectEvent?.Invoke();
        }
        else
        {
            Debug.Log("JAWABAN SALAH");
            vfxIncorrect.gameObject.SetActive(true);

            OnInCorrect?.Invoke();
            OnWrongEvent?.Invoke();
        }

        StartCoroutine(NextSoal());
    }

    void ResetElement()
    {
        isAnswered = false;
        vfxIncorrect.gameObject.SetActive(false);

        for (int i = 0; i < btnMultipleChoices.Length; i++)
        {
            btnMultipleChoices[i].ResetChoice();
        }
    }

    void SetSoal()
    {
        ResetElement();

        framePlaceholderJawaban.gameObject.SetActive(true);
        frameJawaban.gameObject.SetActive(false);

        string soal = soalData[currentSoal].soalText;

        textSoal.text = soal;

        for (int i = 0; i < btnMultipleChoices.Length; i++)
        {
            bool isRight = soalData[currentSoal].pilihanJawaban[i].isRightAnswer;
            string pilihanJawaban = soalData[currentSoal].pilihanJawaban[i].answerNumber.ToString();

            btnMultipleChoices[i].isRightAnswer = isRight;
            btnMultipleChoices[i].txtPilihanJawaban.text = pilihanJawaban;

            if (isRight)
                textFrameJawaban.text = pilihanJawaban;
        }

    }

    IEnumerator NextSoal()
    {
        yield return new WaitForSeconds(1f);
        currentSoal++;

        if (currentSoal <= soalData.Count - 1)
        {
            SetSoal();
        }
        else
        {
            GameFinished();
        }
    }

    bool isSubsded;
    private void GameFinished()
    {
        coverUnTouch.gameObject.SetActive(false);

        if (isLatihanSoal)
        {
            //Submit Score
            progressionManager.SetScoreLatihanSoal(1, correctAnswerTotal);

            UIScoreLatihanSoal.gameObject.SetActive(true);

            UIScoreLatihanSoal.InitializeScore(correctAnswerTotal);

            if (!isSubsded)
            {
                UIScoreLatihanSoal.OnRetry += UIScoreLatihanSoal_OnRetry;
                UIScoreLatihanSoal.OnDone += UIScoreLatihanSoal_OnDone;
            }
            isSubsded = true;
        }
        else
        {
            //Submit Score
            progressionManager.SetScoreKuis(1, correctAnswerTotal);

            UIScoreKuis.gameObject.SetActive(true);

            UIScoreKuis.InitializeScore(correctAnswerTotal);

            if (!isSubsded)
            {
                UIScoreKuis.OnDone += UIScoreKuis_OnDone;
            }
            isSubsded = true;
        }

        if (correctAnswerTotal > 0)
            OnGetAnyStar?.Invoke();
        else
            OnLoseAllStar?.Invoke();
    }

    private void UIScoreKuis_OnDone()
    {
        UIScoreKuis.OnDone -= UIScoreKuis_OnDone;

        Done();
    }

    private void UIScoreLatihanSoal_OnDone()
    {
        UIScoreLatihanSoal.OnRetry -= UIScoreLatihanSoal_OnRetry;
        UIScoreLatihanSoal.OnDone -= UIScoreLatihanSoal_OnDone;

        Done();
    }

    private void UIScoreLatihanSoal_OnRetry()
    {
        Retry();
    }

    void Retry()
    {
        UIScoreLatihanSoal.gameObject.SetActive(false);
        correctAnswerTotal = 0;
        currentSoal = 0;

        if (correctAnswerTotal >= 5)
        {
            //New soal
            currentTypeSoal = 99;
            GenerateSoal();
        }
        else
        {
            //Same soal
        }

        SetSoal();
    }

    void Done()
    {
        coverUnTouch.gameObject.SetActive(true);

        //Back to previous page
    }
}