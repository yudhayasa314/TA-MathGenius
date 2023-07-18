using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DragDropGameManager : MonoBehaviour
{
    ProgressionManager progressionManager;
    public DropSlotUI dropSlotUI;
    public DragUIPieces[] dragUIPieces;

    public List<LatihanSoalSO> soalDatabaseLatihanSoal = new List<LatihanSoalSO>();
    public List<LatihanSoalSO> soalDatabaseKuis = new List<LatihanSoalSO>();

    public List<LatihanSoalSO> soalData = new List<LatihanSoalSO>();

    public MateriBelajar uiPembahasan;
    public DOTweenAnimation vfxCorrect;
    public DOTweenAnimation vfxInCorrect;
    public GameObject coverUnTouch;

    public UIScoreLatihanSoal UIScoreLatihanSoal;
    public UIScoreKuis UIScoreKuis;

    [Header("Status")]
    public int currentSoal;
    DragUIPieces droppedUIPieces;
    bool isConfirmingAnswer;
    bool isLatihanSoal;
    public int correctAnswerTotal;
    int currentTypeSoal = 99;

    public event Action OnCorrect;
    public event Action OnInCorrect;

    public UnityEvent OnCorrectEvent;
    public UnityEvent OnWrongEvent;
    public UnityEvent OnGetAnyStar;
    public UnityEvent OnLoseAllStar;


    private void Start()
    {
        progressionManager = ProgressionManager.instance;

        coverUnTouch.gameObject.SetActive(false);
        //First new soal
        currentTypeSoal = 99;
        
        GenerateSoal();
        SetSoal();

        dropSlotUI.OnDropValid += DropSlotUI_OnDropValid;
        OnCorrect += DragDropGameManager_OnCorrect;
        OnInCorrect += DragDropGameManager_OnInCorrect;
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

            while(currentTypeSoal == tempTypeSoal)
            {
                currentTypeSoal = Random.Range(0, maxRange);
            }
        }

        
        int addition = currentTypeSoal * soalPerType;

        soalData.Clear();

        for (int i = 0; i < soalPerType; i++)
        {
            if(isLatihanSoal)
            {
                soalData.Add(soalDatabaseLatihanSoal[i + addition]);
                uiPembahasan.content[i].audioTextToSpeech = soalDatabaseLatihanSoal[i + addition].clipPembahasan;
                uiPembahasan.content[i].content.transform.GetChild(2).GetComponent<Image>().sprite = soalDatabaseLatihanSoal[i + addition].pembahasanPict;
            }   
            else
                soalData.Add(soalDatabaseKuis[i + addition]);

        }
    }

    private void DragDropGameManager_OnInCorrect()
    {
        vfxInCorrect.gameObject.SetActive(true);

        StartCoroutine(NextSoal());
    }

    private void DragDropGameManager_OnCorrect()
    {
        correctAnswerTotal++;
        vfxCorrect.gameObject.SetActive(true);

        StartCoroutine(NextSoal());
    }

    private void DropSlotUI_OnDropValid(DragUIPieces obj)
    {
        if (isConfirmingAnswer)
            return;

        droppedUIPieces = obj;

        if (obj.isRightAnswer)
        {
            Debug.Log("JAWABAN BENAR");
            OnCorrect?.Invoke();
            OnCorrectEvent?.Invoke();
        }
        else
        {
            Debug.Log("JAWABAN SALAH");
            OnInCorrect?.Invoke();
            OnWrongEvent?.Invoke();
        }

        isConfirmingAnswer = true;
        coverUnTouch.gameObject.SetActive(true);
    }

    void ResetElement()
    {
        for (int i = 0; i < dragUIPieces.Length; i++)
        {
            dragUIPieces[i].ResetPosition();
        }

        vfxCorrect.gameObject.SetActive(false);
        vfxInCorrect.gameObject.SetActive(false);
    }

    void SetSoal()
    {
        isConfirmingAnswer = false;
        coverUnTouch.gameObject.SetActive(false);

        ResetElement();

        string soal = soalData[currentSoal].soalText;

        dropSlotUI.textSoal.text = soal;

        for (int i = 0; i < dragUIPieces.Length; i++)
        {
            bool isRight = soalData[currentSoal].pilihanJawaban[i].isRightAnswer;
            string pilihanJawaban = soalData[currentSoal].pilihanJawaban[i].answerNumber.ToString();

            dragUIPieces[i].isRightAnswer = isRight;
            dragUIPieces[i].txtPilihanJawaban.text = pilihanJawaban;
        }
    }

     
    IEnumerator NextSoal()
    {
        yield return new WaitForSeconds(1f);
        currentSoal++;

        if(currentSoal <= soalData.Count -1)
        {
            SetSoal();
        }
        else
        {
            GameFinished();
        }
    }

    bool isSubsded;
    void GameFinished()
    {
        coverUnTouch.gameObject.SetActive(false);

        if (isLatihanSoal)
        {
            //Submit Score
            progressionManager.SetScoreLatihanSoal(0, correctAnswerTotal);

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
            progressionManager.SetScoreKuis(0, correctAnswerTotal);

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