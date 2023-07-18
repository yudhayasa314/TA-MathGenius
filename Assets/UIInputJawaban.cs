using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class UIInputJawaban : MonoBehaviour
{
    public TextMeshProUGUI textClue;
    public GameObject inputCont;

    public TMP_InputField textInputAnswer;

    bool isAnswerSubmited;
    string rightAnswer;

    public DOTweenAnimation vfxIncorrect;
    public GameObject vfxCorrect;

    public event Action OnAnswerCorrect;
    public event Action OnAnswerIncorrect;

    public void ActivateTextClue(string clue)
    {
        ResetInput();

        inputCont.gameObject.SetActive(false);
        textClue.gameObject.SetActive(true);
        textClue.text = $"{clue}";
    }

    public void ActivateInputField(string answer)
    {
        ResetInput();

        isAnswerSubmited = false;
        rightAnswer = answer;

        textClue.gameObject.SetActive(false);
        inputCont.gameObject.SetActive(true);

        textInputAnswer.onDeselect.AddListener(CheckAnswer);
        textInputAnswer.onSubmit.AddListener(CheckAnswer);
    }

    public void ResetInput()
    {
        vfxCorrect.gameObject.SetActive(false);
        vfxIncorrect.gameObject.SetActive(false);
        textInputAnswer.text = "";
    }

    void CheckAnswer(string answerInput)
    {
        if (answerInput == "")
            return;

        if (isAnswerSubmited)
            return;

        isAnswerSubmited = true;

        if(answerInput == rightAnswer)
        {
            vfxCorrect.gameObject.SetActive(true);
            OnAnswerCorrect?.Invoke();
        }
        else
        {
            vfxIncorrect.gameObject.SetActive(true);
            OnAnswerIncorrect?.Invoke();
        }
    }
}