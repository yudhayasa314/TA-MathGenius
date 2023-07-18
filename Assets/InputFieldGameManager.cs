using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InputFieldGameManager : MonoBehaviour
{
    ProgressionManager progressionManager;

    public TextMeshProUGUI textSoalCerita;
    public UIInputJawaban[] uiInputJawaban;
    public TextMeshProUGUI[] textMathOperation;

    public List<InputFieldSoalSO> soalDatabaseLatihanSoal = new List<InputFieldSoalSO>();
    public List<InputFieldSoalSO> soalDatabaseKuis = new List<InputFieldSoalSO>();
    public List<InputFieldSoalSO> soalData = new List<InputFieldSoalSO>();

    public AudioSource soalClip;
    public MateriBelajar uiPembahasan;
    public GameObject coverUnTouch;

    public UIScoreLatihanSoal UIScoreLatihanSoal;
    public UIScoreKuis UIScoreKuis;

    public event Action OnCorrect;
    public event Action OnInCorrect;

    [Header("Status")]
    public int currentSoal;
    bool isLatihanSoal;
    public int correctAnswerTotal;
    int currentTypeSoal = 99;

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

        for (int i = 0; i < uiInputJawaban.Length; i++)
        {
            uiInputJawaban[i].OnAnswerCorrect += InputFieldGameManager_OnAnswerCorrect;
            uiInputJawaban[i].OnAnswerIncorrect += InputFieldGameManager_OnAnswerIncorrect;
        }

        GenerateSoal();
        SetSoal();
    }

    private void OnDisable()
    {
        for (int i = 0; i < uiInputJawaban.Length; i++)
        {
            uiInputJawaban[i].OnAnswerCorrect -= InputFieldGameManager_OnAnswerCorrect;
            uiInputJawaban[i].OnAnswerIncorrect -= InputFieldGameManager_OnAnswerIncorrect;
        }
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

    private void InputFieldGameManager_OnAnswerIncorrect()
    {
        Debug.Log("Wrong");
        OnInCorrect?.Invoke();
        OnWrongEvent?.Invoke();


        StartCoroutine(NextSoal());

    }

    private void InputFieldGameManager_OnAnswerCorrect()
    {
        correctAnswerTotal++;
        Debug.Log("Correct");
        OnCorrect?.Invoke();
        OnCorrectEvent?.Invoke();


        StartCoroutine(NextSoal());

    }

    void ResetElement()
    {
        for (int i = 0; i < uiInputJawaban.Length; i++)
        {
            uiInputJawaban[i].ResetInput();
        }
    }

    public void SetSoal()
    {
        ResetElement();

        InputFieldSoalSO soalSO = soalData[currentSoal];

        soalClip.clip = soalSO.clipSoal;
        textSoalCerita.text = soalSO.soalCerita;

        if(soalSO.firstMathOperation != "")
        {
            textMathOperation[0].transform.parent.gameObject.SetActive(true);
            textMathOperation[0].text = soalSO.firstMathOperation;
        }
        else
        {
            textMathOperation[0].transform.parent.gameObject.SetActive(false);
        }

        if (soalSO.secondMathOperation != "")
        {
            textMathOperation[1].transform.parent.gameObject.SetActive(true);
            textMathOperation[1].text = soalSO.secondMathOperation;
        }
        else
        {
            textMathOperation[1].transform.parent.gameObject.SetActive(false);
        }

        for (int i = 0; i < soalSO.clue.Length; i++)
        {
            InputFieldSoalSO.InputClue clue = soalSO.clue[i];

            uiInputJawaban[i].gameObject.SetActive(clue.isActive);

            if(clue.clue == "")
            {
                //Bearti harus di isi player
                uiInputJawaban[i].ActivateInputField(soalSO.answer);
            }
            else
            {
                //Bearti clue
                uiInputJawaban[i].ActivateTextClue(clue.clue);
            }
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
    void GameFinished()
    {
        coverUnTouch.gameObject.SetActive(false);

        if (isLatihanSoal)
        {
            //Submit Score
            progressionManager.SetScoreLatihanSoal(2, correctAnswerTotal);

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
            progressionManager.SetScoreKuis(2, correctAnswerTotal);

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
