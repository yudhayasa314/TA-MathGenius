using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioManager audioManager;
    public string startAudio;
    public bool stopAllAudioOnStart;

    private void OnEnable()
    {
        audioManager = AudioManager.instance;
    }

    private void Start()
    {
        if(startAudio != "")
        {
            Play(startAudio);
        }

        if (stopAllAudioOnStart)
        {
            audioManager.StopAll();
        }
    }

    public void Play(string clipName)
    {
        if(audioManager != null)
            audioManager.Play(clipName);
    }
}
