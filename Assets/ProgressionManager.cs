using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager instance;

    public string playerName;

    public TypeSoal typeSoal;
    public LevelSoal levelSoal;
    //public OperationSoal operationSoal;

    public int[] scoreLatihanSoal = new int[3];
    public int[] scoreKuis = new int[3];
    public int GetTotalScoreLatihanSoal
    {
        get
        {
            int total = 0;
            for (int i = 0; i < scoreLatihanSoal.Length; i++)
            {
                total += scoreLatihanSoal[i];
            }

            return total;
        }
    }

    public int GetTotalScoreKuis
    {
        get
        {
            int total = 0;
            for (int i = 0; i < scoreKuis.Length; i++)
            {
                total += scoreKuis[i];
            }

            return total;
        }
    }

    public AudioClip[] allVoiceRecorded = new AudioClip[10];

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ReferenceAudioClip();
    }

    void Load()
    {

        if (ES3.KeyExists("playerName"))
            playerName = ES3.Load<string>("playerName");

        if (ES3.KeyExists("scoreLatihanSoal"))
            scoreLatihanSoal = ES3.Load<int[]>("scoreLatihanSoal");

        if (ES3.KeyExists("scoreKuis"))
            scoreKuis = ES3.Load<int[]>("scoreKuis");
    }

    void Save()
    {
        ES3.Save("playerName", playerName);
        ES3.Save("scoreLatihanSoal", scoreLatihanSoal);
        ES3.Save("scoreKuis", scoreKuis);
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
            Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void SaveRecordedMemo(int indexNumber, AudioClip clip)
    {
        allVoiceRecorded[indexNumber] = clip;
    }

    void ReferenceAudioClip()
    {
        for (int i = 0; i < allVoiceRecorded.Length; i++)
        {
            StartCoroutine(LoadFile($"Record: {i}.wav", i));
        }
    }

    private IEnumerator LoadFile(string fileName, int index)
    {
        string fullpath = Path.Combine(Application.persistentDataPath, fileName);
        print("LOADING CLIP " + fullpath);

        if (!System.IO.File.Exists(fullpath))
        {
            print("DIDN'T EXIST: " + fullpath);
            yield break;
        }

        AudioClip temp = null;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + fullpath, AudioType.WAV))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                temp = DownloadHandlerAudioClip.GetContent(www);
            }
        }

        allVoiceRecorded[index] = temp;

        //AudioSource audio = GetComponent<AudioSource>();
        //audio.clip = temp;
        //audio.Play();
    }


    public void SetPlayerName(string name)
    {
        playerName = name;

        Save();
    }

    public void SetTypeSoal(TypeSoal type)
    {
        typeSoal = type;
    }

    public void SetLevelSoal(LevelSoal level)
    {
        levelSoal = level;
    }

    //public void SetOperationSoal(OperationSoal soal)
    //{
    //    operationSoal = soal;
    //}

    public void SetScoreLatihanSoal(int indexLevel, int star)
    {
        scoreLatihanSoal[indexLevel] = star;

        Save();
    }

    public void SetScoreKuis(int indexLevel, int score)
    {
        int multiplied = score * 10;
        scoreKuis[indexLevel] = multiplied;

        Save();
    }
}
