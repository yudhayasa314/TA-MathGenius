using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Soal Tipe Input Field", fileName = "Soal Input")]
public class InputFieldSoalSO : ScriptableObject
{
    [TextArea]
    public string soalCerita;
    public AudioClip clipSoal;
    public Sprite pembahasanPict;
    public AudioClip clipPembahasan;

    [Tooltip("Max Array Leght: 4")]
    public InputClue[] clue = new InputClue[4];
    public string firstMathOperation;
    public string secondMathOperation;

    public string answer;

    [System.Serializable]
    public class InputClue
    {
        public string clue;
        public bool isActive;
    }
}
