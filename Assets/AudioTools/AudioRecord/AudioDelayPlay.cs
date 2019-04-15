using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDelayPlay : MonoBehaviour {

    public delegate void AudioPlayHandler();
    public event AudioPlayHandler eventAudioOnStart;
    public event AudioPlayHandler eventAudioOnComplete;


	AudioSource audioSrc;

	public double gain = 0.05;


	// Use this for initialization
	void Start () {
		audioSrc = this.gameObject.GetComponent<AudioSource> ();
		audioSrc.clip = null;
		
		audioSrc.Play ();
	}

	[SerializeField] RecordVoice recordVoice;
	[SerializeField] bool playing = false;
	
	List<float[]> recordAudioData = new List<float[]>();

	int index = 0;

	// Update is called once per frame
	void OnAudioFilterRead (float[] data, int channels)
	{
		if (!playing) {
			// mute
			for (int i = 0; i < data.Length; i++) {
				data[i] = 0;
			}
			return;
		}

		// 
		// 再生しながら録音したい場合は有効に
//		if (recordVoice.IsRecording ()) {
//			if (recordVoice.recordAudioData.Count > 1) {
//				recordAudioData.Add (recordVoice.recordAudioData [recordVoice.recordAudioData.Count - 1]);
//			}
//		}
		//

		// play sound -------------
//		if (recordAudioData.Count > 0) {
//			float[] recordData = recordAudioData [0];
//
//			// copy and apply sound data
//			for (int i = 0; i < data.Length; i++) { 
//				float p = recordData [i];
//				data [i] = (float)(gain * p);
//			}
//
//			recordAudioData.RemoveAt (0);
//		}else{
//			// EndPlay
//			playing = false;
//		}

		if (index < recordAudioData.Count) {
			float[] recordData = recordAudioData [index];

			// copy and apply sound data
			for (int i = 0; i < data.Length; i++) { 
				float p = recordData [i];
				data [i] = (float)(gain * p);
			}
			
			index++;
		}else{
			// EndPlay
			playing = false;
            
            if (eventAudioOnComplete != null) {
                eventAudioOnComplete();
            }
		}
	}

    public float GetProgress()
    {
        return (float)index / (recordAudioData.Count);
    }

	public void SetRecordData(List<float[]> recordAudioData_)
	{
		recordAudioData.Clear ();
		recordAudioData.AddRange(recordAudioData_);
	}
		
	public void StartPlay()
	{
		index = 0;
		playing = true;

        if(eventAudioOnStart != null) {
            eventAudioOnStart();
        }
	}

	public bool IsPlaying()
	{
		return playing;
	}
}
