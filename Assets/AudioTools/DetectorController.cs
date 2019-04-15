using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour {

    [SerializeField] Detector detector;
    [SerializeField] AudioDelayPlay audioDelayPlay;
    [SerializeField] RecordVoice recordVoice;

    [SerializeField] MusicInput audioDelayMusicInput;

    [SerializeField] bool isDelayPlaying = false;
    
	// Use this for initialization
	void Start () {
        detector.eventRecordAudioStart += Detector_EventRecordAudioStart;
        detector.eventRecordAudioComplete += Detector_EventRecordAudioComplete;
        detector.eventRecordAudioTimeout += Detector_EventRecordAudioTimeout;

        audioDelayPlay.eventAudioOnComplete += AudioDelayPlay_EventAudioOnComplete;

        recordVoice.enableRecord = false;
        recordVoice.StartMicrophone();
	}
	
	// Update is called once per frame
	void Update () {
        if(isDelayPlaying){
            float volume = audioDelayMusicInput.GetVolume01();
            float pitch = audioDelayMusicInput.GetPitch01();
            Debug.LogWarning(volume.ToString() + " / "+pitch.ToString());
        }
	}

    void Detector_EventRecordAudioStart(float startTime)
    {
        // 録音開始
        Debug.Log("start: " + startTime);
    }

    List<float[]> current_recData;
    void Detector_EventRecordAudioComplete(List<float[]> recData, float duration)
    {
        // 録音完了
        Debug.Log(recData.Count + " / duration: "+duration);
        //detector.StartReplay(recData);
        detector.StopDetect();

        // save recData
        current_recData = recData;
        // play
        PlayRecordAudio();
    }

    public void PlayRecordAudio()
    {
        // 再生
        // play
        audioDelayPlay.SetRecordData(current_recData);
        audioDelayPlay.StartPlay();

        isDelayPlaying = true;
    }

    void Detector_EventRecordAudioTimeout()
    {
        // タイムアウト
        Debug.Log("timeout");
    }

    void AudioDelayPlay_EventAudioOnComplete()
    {
        // 録音した音声の再生完了
        Debug.Log("AudioDelayPlay_EventAudioOnComplete");
        isDelayPlaying = false;
    }
   
#region API
    public void StartDetect()
    {
        Debug.Log("StartDetect");
        detector.StartDetect(); 
    }
    public void StopDetect()
    {
        Debug.Log("StopDetect");
        detector.StopDetect();
    }
#endregion

    [SerializeField] Rect drawRect = new Rect(10,10,100,100);

	private void OnGUI()
	{
        GUILayout.BeginArea(drawRect);
        bool enableDetect = detector.IsEnableDetect();
        GUI.color = enableDetect ? Color.red : Color.white;
        GUILayout.Label("IsEnableDetect: " + enableDetect);
        bool isRecording = detector.IsRecording();
        GUI.color = isRecording ? Color.red : Color.white;
        GUILayout.Label("isRecording: " + isRecording);
        GUI.color = isDelayPlaying ? Color.red : Color.white;
        GUILayout.Label("isDelayPlaying: "+isDelayPlaying);
        GUI.color = Color.white;
        if(GUILayout.Button("StartDetect")){
            StartDetect();   
        }
        if (GUILayout.Button("StopDetect")){
            StopDetect();
        }
        GUILayout.EndArea();
	}
}
