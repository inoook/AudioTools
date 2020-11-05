using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordVoice_test : MonoBehaviour {

	[SerializeField] AudioRecorder audioRecorder = null;

	[SerializeField] float delayTime = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[SerializeField] Rect drawRect;

	void OnGUI()
	{
		GUILayout.BeginArea (drawRect);
		GUILayout.Label ("IsRecording: "+audioRecorder.IsRecording());

		if (GUILayout.Button ("StartRecord and Play")) {

			// サウンドデータを保存して、audioDelayPlay で再生
			audioRecorder.StartRecord();

			Invoke ("PlayCopyDelayAudio", delayTime); // 5秒ずらして録音したサウンド再生
			Invoke ("PlayCopyDelayAudio2", delayTime + 1); //ずらして録音したサウンド再生
		}

		GUILayout.EndArea ();
	}

	[SerializeField] AudioDataPlayer audioDelayPlay;
	[SerializeField] AudioDataPlayer audioDelayPlay2;
	void PlayCopyDelayAudio()
	{
		audioDelayPlay.SetRecordData (audioRecorder.GetRecData());
		audioDelayPlay.Play ();
	}

	void PlayCopyDelayAudio2()
	{
		audioDelayPlay2.SetRecordData (audioRecorder.GetRecData());
		audioDelayPlay2.Play ();
	}
}
