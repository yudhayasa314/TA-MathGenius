using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Latihan Soal", fileName = "Soal")]
public class LatihanSoalSO : ScriptableObject
{
    [TextArea]
    public string soalText;

    public Sprite pembahasanPict;
    public AudioClip clipPembahasan;

    public Choices[] pilihanJawaban = new Choices[4];


    [Serializable]
    public class Choices
    {
        public bool isRightAnswer;
        public int answerNumber;
    }
}
