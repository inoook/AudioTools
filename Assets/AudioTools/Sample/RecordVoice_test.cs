using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordVoice_test : MonoBehaviour {

	[SerializeField] RecordVoice recordVoice;

	[SerializeField] float delayTime = 5;

	// Use this for initialization
	void Start () {
		recordVoice.enableRecord = false;
		recordVoice.StartMicrophone ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[SerializeField] Rect drawRect;

	void OnGUI()
	{
		GUILayout.BeginArea (drawRect);
//		if (GUILayout.Button ("StartRecord")) {
//			recordVoice.StartRecord ();
//		}
//		if (GUILayout.Button ("StopRecord")) {
//			recordVoice.StopRecord ();
//		}

		GUILayout.Label ("IsRecording: "+recordVoice.IsRecording());

		if (GUILayout.Button ("StartRecord and Play")) {

			// サウンドデータを保存して、audioDelayPlay で再生
			recordVoice.StartRecord();

			Invoke ("PlayCopyDelayAudio", delayTime); // 5秒ずらして録音したサウンド再生
			Invoke ("PlayCopyDelayAudio2", delayTime + 1); //ずらして録音したサウンド再生
		}

		GUILayout.EndArea ();
	}

	[SerializeField] AudioDelayPlay audioDelayPlay;
	[SerializeField] AudioDelayPlay audioDelayPlay2;
	void PlayCopyDelayAudio()
	{
		audioDelayPlay.SetRecordData (recordVoice.recordAudioData);
		audioDelayPlay.StartPlay ();
	}

	void PlayCopyDelayAudio2()
	{
		audioDelayPlay2.SetRecordData (recordVoice.recordAudioData);
		audioDelayPlay2.StartPlay ();
	}
}
