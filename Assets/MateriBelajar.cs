using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MateriBelajar : MonoBehaviour
{
    public AudioSource audioSource;
    public Button previousButton;
    public Button nextButton;
    public Button audioButton;

    [System.Serializable]
    public class Content
    {
        public GameObject content;
        public AudioClip audioTextToSpeech;
    }

    public Content[] content;

    [Header("Status")]
    public int currentContentIndex;
    public AudioClip currentTextToSpeechClip;

    public UnityEvent OnCloseMateri;
    public UnityEvent OnFinishMateri;

    private void OnEnable()
    {
        currentContentIndex = -1;
        NextContent();
    }

    void DisableAllContent()
    {
        audioSource.Stop();

        foreach (var item in content)
        {
            item.content.gameObject.SetActive(false);
        }
    }

    public void PlayTextToSpeech()
    {
        audioSource.Stop();
        audioSource.clip = currentTextToSpeechClip;

        try
        {
            audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log("AUDIO Text2Speech" + e);
        }
    }

    public void NextContent()
    {
        DisableAllContent();

        currentContentIndex++;
        if (currentContentIndex >= content.Length)
        {
            currentContentIndex = content.Length - 1;
            //finish materi
            OnFinishMateri?.Invoke();
            return;
        }
        else
        {
            nextButton.gameObject.SetActive(true);
        }

        ActiveContent(currentContentIndex);
    }

    public void PreviousContent()
    {
        DisableAllContent();

        currentContentIndex--;

        if (currentContentIndex < 0)
        {
            currentContentIndex = 0;
            //back
            OnCloseMateri?.Invoke();
            return;
        }

        ActiveContent(currentContentIndex);
    }

    void ActiveContent(int index)
    {
        DisableAllContent();

        Content activeContent = content[index];
        currentTextToSpeechClip = activeContent.audioTextToSpeech;

        audioButton.gameObject.SetActive(currentTextToSpeechClip != null);
        activeContent.content.gameObject.SetActive(true);
    }
}
