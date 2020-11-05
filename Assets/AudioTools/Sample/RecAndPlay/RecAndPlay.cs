using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RecAndPlay : MonoBehaviour {

    [SerializeField] AudioRecorder audioRecorder = null;
    [SerializeField] AudioDataPlayer audioDataPlayer = null;

	// Use this for initialization
	void Start () {
        audioDataPlayer.eventAudioOnComplete += () => {
            // 録音データの再生完了
            Debug.LogWarning("audioDataPlayer Complete");
        };
    }

    void StartRecord()
    {
        audioRecorder.StartRecord();
    }

    void StopRecord()
    {
        audioRecorder.StopRecord();
    }

    void PlayRecordVice()
    {
        List<float[]> recData = audioRecorder.GetRecData();
        audioDataPlayer.SetRecordData(recData);
        audioDataPlayer.Play();
    }

    void Clear()
    {
        audioRecorder.ClearRecordData();
        audioDataPlayer.Clear();
    }

    [SerializeField] Rect drawRect = new Rect(10,10,200,200);
	private void OnGUI()
	{
        GUILayout.BeginArea(drawRect);

        bool isRecording = audioRecorder.IsRecording();
        GUILayout.Label("isRecording: "+ isRecording + " / "+ audioRecorder.GetRecData().Count);
        if (!isRecording)
        {
            if (GUILayout.Button("StartRecord"))
            {
                StartRecord();
            }
        }else{
            if (GUILayout.Button("StopRecord"))
            {
                StopRecord();
            }
        }

        GUILayout.Space(20);
        //
        GUILayout.Label("progress");

        bool isPlaying = audioDataPlayer.IsPlaying();
        GUILayout.Label("isPlaying: "+isPlaying + " / "+ audioDataPlayer.GetCurrentIndex());
        GUILayout.HorizontalSlider(audioDataPlayer.GetProgress(), 0, 1f);

        if(GUILayout.Button("RecDataPlay: "+ isPlaying)){
            PlayRecordVice();
        }
        //
        if (GUILayout.Button("Clear"))
        {
            Clear();
        }

        GUILayout.EndArea();
	}
}
