using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioRec : MonoBehaviour
{
    public AudioClip myAudioClip;

    public void Record()
    {
        myAudioClip = Microphone.Start(null, false, 10, 44100);
    }

    public void Save(string filename)
    {
        SavWav.Save(filename, myAudioClip);
    }

    public void PlayRecordedAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = myAudioClip;
        audio.Play();
    }

    public void StopPreview()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
    }

    public void PlaySearch(string fileName)
    {
        StartCoroutine(LoadFile(fileName += ".wav"));
    }

    private IEnumerator LoadFile(string fileName)
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

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = temp;
        audio.Play();
    }
}
