using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoPlayer : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        TryGetComponent(out audioSource);
    }

    public void PlayMemo(int index)
    {
        audioSource.Stop();
        AudioClip clip = ProgressionManager.instance.allVoiceRecorded[index];
        audioSource.PlayOneShot(clip);
    }
}
