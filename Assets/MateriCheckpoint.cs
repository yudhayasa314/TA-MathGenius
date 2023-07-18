using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateriCheckpoint : MonoBehaviour
{
    public GameObject[] checkpointObject;
    public string namaMateri;
    public bool[] materiFinish;

    private void OnEnable()
    {
        UpdateCheckpoint();
    }

    public void FinishMateri(int indexMateriFinished)
    {
        materiFinish[indexMateriFinished] = true;
        SaveMateriFinish(indexMateriFinished, namaMateri);
        UpdateCheckpoint();
    }

    void UpdateCheckpoint()
    {
        for (int i = 0; i < materiFinish.Length; i++)
        {
            bool isFinish = IsMateriFinished(i, namaMateri);
            materiFinish[i] = isFinish;
            checkpointObject[i].gameObject.SetActive(materiFinish[i]);
        }
    }

    public static void SaveMateriFinish(int index, string namaMateri)
    {
        // Menyimpan status level terbuka dengan nilai 1
        PlayerPrefs.SetInt("materi" + namaMateri + index, 1);

        // Menyimpan PlayerPrefs
        PlayerPrefs.Save();
    }

    public static bool IsMateriFinished(int index, string namaMateri)
    {
        // Memeriksa apakah status level terbuka dengan nilai 1
        return PlayerPrefs.GetInt("materi" + namaMateri + index, 0) == 1;
    }
}
