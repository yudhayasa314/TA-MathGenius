using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TypeSoal
{
    LATIHAN_SOAL,
    KUIS
}

public enum LevelSoal
{
    PEMULA,
    MENENGAH,
    LANJUTAN
}

//public enum OperationSoal
//{
//    PENJUMLAHAN,
//    PENGURANGAN,
//    PERKALIAN,
//    PEMBAGIAN
//}


public class SceneHandler : MonoBehaviour
{
    ProgressionManager progressionManager;


    private void Start()
    {
        progressionManager = ProgressionManager.instance;
    }

    #region Scene Soal
    public void SetTypeSoal(int indexType)
    {
        if(indexType == 0)
        {
            progressionManager.SetTypeSoal(TypeSoal.LATIHAN_SOAL);
        }
        else
        {
            progressionManager.SetTypeSoal(TypeSoal.KUIS);
        }
    }

    public void SetLevelSoal(int indexLevel)
    {
        if (indexLevel == 0)
        {
            progressionManager.SetLevelSoal(LevelSoal.PEMULA);
        }
        else if (indexLevel == 1)
        {
            progressionManager.SetLevelSoal(LevelSoal.MENENGAH);
        }
        else
        {
            progressionManager.SetLevelSoal(LevelSoal.LANJUTAN);
        }
    }

    //public void SetOperationSoal(int indexOperation)
    //{
    //    if (indexOperation == 0)
    //    {
    //        progressionManager.SetOperationSoal(OperationSoal.PENJUMLAHAN);
    //    }
    //    else if (indexOperation == 1)
    //    {
    //        progressionManager.SetOperationSoal(OperationSoal.PENGURANGAN);
    //    }
    //    else if (indexOperation == 2)
    //    {
    //        progressionManager.SetOperationSoal(OperationSoal.PERKALIAN);
    //    }
    //    else
    //    {
    //        progressionManager.SetOperationSoal(OperationSoal.PEMBAGIAN);
    //    }
    //}

    public void GoToSoal()
    {
        string soalLanjutan = "Game Latihan Soal Lanjutan";
        string soalMenengah = "Game Latihan Soal Menengah";
        string soalPemula = "Game Latihan Soal Pemula";

        string sceneName = "";

        if(progressionManager.levelSoal == LevelSoal.PEMULA)
        {
            sceneName = soalPemula;
        }
        else if (progressionManager.levelSoal == LevelSoal.MENENGAH)
        {
            sceneName = soalMenengah;
        }
        else
        {
            sceneName = soalLanjutan;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void DoneSoal()
    {
        string latsol = "Latihan Soal";
        string kuis = "Kuis";

        string sceneName = "";

        if (progressionManager.typeSoal == TypeSoal.LATIHAN_SOAL)
            sceneName = latsol;
        else if (progressionManager.typeSoal == TypeSoal.KUIS)
            sceneName = kuis;
        else
            sceneName = "Menu";

        SceneManager.LoadScene(sceneName);
    }

    #endregion


    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}