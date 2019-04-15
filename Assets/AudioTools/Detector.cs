using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// vThreshold 以上の値のときに録音開始、
/// それ以降、値が小さくなると録音停止し、録音した音源を再生。
/// </summary>
public class Detector : MonoBehaviour {

    public delegate void RecordAudioStartHandler(float time);
    public event RecordAudioStartHandler eventRecordAudioStart;

    public delegate void RecordAudioCompleteHandler(List<float[]> recData, float duration);
    public event RecordAudioCompleteHandler eventRecordAudioComplete;

    public delegate void RecordAudioTimeoutHandler();
    public event RecordAudioTimeoutHandler eventRecordAudioTimeout;

	[SerializeField] MusicInput musicInput;
	[SerializeField] float v;
    [Tooltip("サウンドの録音開始する閾値")]
	[SerializeField] float vThreshold = 0.2f;

    [SerializeField] bool isStartRecord = false;

    [SerializeField] RecordVoice recordVoice;

    [Tooltip("最大の録音時間")]
    [SerializeField] float recLimit = 10;
    [Tooltip("最低でもこの時間は音を録音")]
    [SerializeField] float recMinTime = 5;

    [SerializeField] bool enableDetect = false;

    [SerializeField] float detectTimeout = 15;

    private float startTime;
	private float detectStartTime;

	// Use this for initialization
	void Start () {
        // 1秒間のcount数を取得するため
		Invoke (((System.Action)StartRecord).Method.Name, 1.0f);
        Invoke (((System.Action)Check).Method.Name, 2.0f);
	}

	void Check()
	{
		Debug.Log (recordVoice.debug_count);
        StopRecord();
	}

	// Update is called once per frame
	void Update () {
        if(!enableDetect){
            return; 
        }

		v = musicInput.GetVolume01 ();
		if (!isStartRecord) {
			if (v > vThreshold) {
				Debug.Log ("Start");
				isStartRecord = true;
				startTime = Time.time;
                debug_duration = 0;

                if(eventRecordAudioStart != null){
                    eventRecordAudioStart(startTime);
                }
//				StartRecord ();
			}

            // timeOut --
            float detectingTime = Time.time - detectStartTime;
            if (detectingTime > detectTimeout)
            {
                if (eventRecordAudioTimeout != null){
                    eventRecordAudioTimeout();
                }
                //
                StopDetect();
            }
		}
		if (isStartRecord) {
            float duration = Time.time - startTime;
            debug_duration = duration;
            if (duration > recMinTime && (v < vThreshold * 0.5f || duration > recLimit) ) {
				isStartRecord = false;
				
				Debug.Log ("End: " + duration);
//				EndRecord ();
				PlayRecordData (duration);
			}
		}
	}

    [SerializeField] public float debug_duration;

	void StartRecord()
	{
		recordVoice.ClearRecordData ();
		recordVoice.StartRecord ();
	}

	void StopRecord()
	{
		recordVoice.StopRecord ();
	}

    [SerializeField] public float delayTime = 1.0f;

	void PlayRecordData(float time)
	{
		int range = ((int)time+1) * 43;
        int dataEnd = recordVoice.recordAudioData.Count;
        int dataCount = Mathf.Min(dataEnd, range);
        int startIndex = dataEnd - dataCount;
		List<float[]> recData = recordVoice.recordAudioData.GetRange(startIndex, dataCount);

        if(eventRecordAudioComplete != null){
            eventRecordAudioComplete(recData, time+1);
        }

		//delayPlay0.SetRecordData (recData);
		//delayPlay0.StartPlay ();
	}

    //public void StartReplay(List<float[]> recData)
    //{
    //    delayPlay0.SetRecordData(recData);
    //    delayPlay0.StartPlay();
    //}

    public void StartDetect()
    {
        detectStartTime = Time.time;

        enableDetect = true;
        StartRecord();
    }

    public void StopDetect()
    {
        enableDetect = false;
        StopRecord();
    }

    public bool IsRecording()
    {
        return isStartRecord;
    }

    public bool IsEnableDetect()
    {
        return enableDetect;
    }

	//[SerializeField] Rect drawRect;

	//void OnGUI()
	//{
	//	GUILayout.BeginArea (drawRect);
	//	GUILayout.Label (this.gameObject.name);
	//	if (GUILayout.Button ("RePlay")) {
			
	//		delayPlay0.StartPlay ();
	//		Invoke ("DelayPlay", 1);
	//	}

	//	GUILayout.EndArea ();
	//}
}
