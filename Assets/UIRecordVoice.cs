using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecordVoice : MonoBehaviour
{
    int numberCounter = 0;
    public string nameFile;
    public AudioRec audioRecorder;

    public TextMeshProUGUI textNumber;
    public Image imgRecord;
    public Color defaultRecordColor;
    public Color recordingColor;

    public GameObject[] recPreviewCont;
    public GameObject panelFinishAllRecord;

    bool isRecording;

    private void Awake()
    {
        panelFinishAllRecord.gameObject.SetActive(false);
        DefaultUI();
    }

    void DefaultUI()
    {
        foreach (var item in recPreviewCont)
        {
            item.gameObject.SetActive(false);
        }

        imgRecord.gameObject.SetActive(true);
        imgRecord.color = defaultRecordColor;

        textNumber.text = $"{numberCounter + 1}";
    }

    public void StartRecord()
    {
        audioRecorder.StopPreview();

        isRecording = true;
        imgRecord.color = recordingColor;

        audioRecorder.Record();
    }

    public void FinishRecord()
    {
        audioRecorder.StopPreview();

        isRecording = false;
        imgRecord.color = defaultRecordColor;

        imgRecord.gameObject.SetActive(false);
        foreach (var item in recPreviewCont)
        {
            item.gameObject.SetActive(true);
        }

        audioRecorder.Save(GetFileName);
    }

    public void RestartRecord()
    {
        audioRecorder.StopPreview();

        imgRecord.gameObject.SetActive(true);
        foreach (var item in recPreviewCont)
        {
            item.gameObject.SetActive(false);
        }
    }


    public void PreviewRecorded()
    {
        audioRecorder.StopPreview();

        audioRecorder.PlayRecordedAudio();
    }

    public void SaveRecord()
    {
        //store
        ProgressionManager.instance.SaveRecordedMemo(numberCounter, audioRecorder.myAudioClip);
        audioRecorder.StopPreview();
        //Next number

        numberCounter++;
        DefaultUI();

        CheckIfAllDone();
    }

    void CheckIfAllDone()
    {
        if(numberCounter >= 10)
        {
            panelFinishAllRecord.gameObject.SetActive(true);
        }
    }

    string GetFileName
    {
        get
        {
            nameFile = $"Record: {numberCounter}";
            return nameFile;
        }
    }
}
