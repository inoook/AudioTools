using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RecAndPlay : MonoBehaviour {

    [SerializeField] RecordVoice recordVoice;
    [SerializeField] AudioDelayPlay audioDelayPlay;
    [SerializeField] AudioMixer mixer;

    [SerializeField] float pitch = 1f;
    [SerializeField] float pitchShift;

	// Use this for initialization
	void Start () {
        recordVoice.StartMicrophone();

        audioDelayPlay.eventAudioOnComplete += () => {
            Debug.LogWarning("Complete");
        };

    }
	
	// Update is called once per frame
	void Update () {
        mixer.SetFloat("Pitch", pitch);
        mixer.SetFloat("PitchShift", pitchShift);
	}

    void PlayRecordVice()
    {
        List<float[]> recData = recordVoice.recordAudioData;
        audioDelayPlay.SetRecordData(recData);
        audioDelayPlay.StartPlay();
    }

    void Clear()
    {
        recordVoice.ClearRecordData();
    }

    [SerializeField] Rect drawRect = new Rect(10,10,200,200);
	private void OnGUI()
	{
        GUILayout.BeginArea(drawRect);

        bool isRecording = recordVoice.IsRecording();
        if (!isRecording)
        {
            if (GUILayout.Button("StartRecord"))
            {
                recordVoice.StartRecord();
            }
        }else{
            if (GUILayout.Button("EndRecord"))
            {
                recordVoice.StopRecord();
            }
        }

        GUILayout.Space(20);
        //
        if(GUILayout.Button("PlayRecordVice: "+ audioDelayPlay.IsPlaying())){
            PlayRecordVice();
        }
        //
        GUILayout.Space(20);

        if (GUILayout.Button("ClearRecordData"))
        {
            recordVoice.ClearRecordData();
        }

        GUILayout.EndArea();
	}
}
