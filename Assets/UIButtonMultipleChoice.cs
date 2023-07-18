using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonMultipleChoice : MonoBehaviour
{
    Button btn;

    public bool isRightAnswer;
    public TextMeshProUGUI txtPilihanJawaban;

    public GameObject vfxCorrect;

    public event System.Action<bool> OnAnswered;

    private void Awake()
    {
        ResetChoice();
        TryGetComponent(out btn);
    }

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(Answer);
    }

    void Answer()
    {
        OnAnswered?.Invoke(isRightAnswer);

        if (isRightAnswer)
        {
            vfxCorrect.gameObject.SetActive(true);
        }
    }

    public void ResetChoice()
    {
        vfxCorrect.gameObject.SetActive(false);

    }
}