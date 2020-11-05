using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour {

    [SerializeField] Detector detector = null;
    [SerializeField] AudioDataPlayer audioDataPlayer = null;
    [SerializeField] AudioRecorder audioRecorder = null;

    [SerializeField] AudioAnalyzer audioDelayAnalyser = null;

    [SerializeField] bool isDelayPlaying = false;
    
	// Use this for initialization
	void Start () {
        detector.eventRecordAudioStart += Detector_EventRecordAudioStart;
        detector.eventRecordAudioComplete += Detector_EventRecordAudioComplete;
        detector.eventRecordAudioTimeout += Detector_EventRecordAudioTimeout;

        audioDataPlayer.eventAudioOnComplete += AudioDelayPlay_EventAudioOnComplete;
	}
	
	// Update is called once per frame
	void Update () {
        if(isDelayPlaying){
            float volume = audioDelayAnalyser.GetVolume();
            float pitch = audioDelayAnalyser.GetPitchHertz();
            Debug.LogWarning(volume.ToString() + " / "+pitch.ToString());
        }
	}

    void Detector_EventRecordAudioStart(float startTime)
    {
        // 録音開始
        Debug.Log("start: " + startTime);
        audioRecorder.StartRecord();
    }

    List<float[]> current_recData;
    void Detector_EventRecordAudioComplete(float duration)
    {
        // 録音完了
        Debug.Log("duration: "+duration);
        //detector.StartReplay(recData);
        detector.StopDetect();

        // play
        PlayRecordAudio();
    }

    public void PlayRecordAudio()
    {
        // 再生
        // play
        current_recData = audioRecorder.GetRecData();
        audioDataPlayer.SetRecordData(current_recData);
        audioDataPlayer.Play();

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
   
    #region public
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
    public void PlayRecData()
    {
        Debug.Log("PlayRecData");
        audioDataPlayer.Play();
    }
    #endregion

    [SerializeField] Rect drawRect = new Rect(10,10,100,100);

	private void OnGUI()
	{
        GUILayout.BeginArea(drawRect);
        bool enableDetect = detector.IsEnableDetect();
        GUI.color = enableDetect ? Color.red : Color.white;
        GUILayout.Label("IsEnableDetect: " + enableDetect);
        bool isRecording = detector.IsDetected();
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
        if (GUILayout.Button("PlayRecData"))
        {
            PlayRecData();
        }
        GUILayout.EndArea();
	}
}
