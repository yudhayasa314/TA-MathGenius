using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFX_ButtonClicked : MonoBehaviour
{
    //mendeteksi otomatis jika button di klik maka akan menjalankau efeek audio
    Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ChangeFunction);
    }

    void ChangeFunction()
    {
        AudioManager.instance.Play("Click");
    }
}
