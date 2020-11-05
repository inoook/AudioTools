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

    public delegate void RecordAudioCompleteHandler(float duration);
    public event RecordAudioCompleteHandler eventRecordAudioComplete;

    public delegate void RecordAudioTimeoutHandler();
    public event RecordAudioTimeoutHandler eventRecordAudioTimeout;

	[SerializeField] AudioAnalyzer audioAnalyzer = null;
	[SerializeField] float currentVolume = 0;
    [Tooltip("サウンドの録音開始する閾値")]
	[SerializeField] float volumeThreshold = 0.2f;

    [SerializeField] bool isDetected = false;

    //[SerializeField] AudioRecorder audioRecorder = null;

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
		//Invoke (((System.Action)StartRecord).Method.Name, 1.0f);
  //      Invoke (((System.Action)Check).Method.Name, 2.0f);
	}

	void Check()
	{
		//Debug.Log (audioRecorder.debug_count);
        //StopRecord();
	}

	// Update is called once per frame
	void Update () {
        if(!enableDetect){
            return; 
        }

		currentVolume = audioAnalyzer.GetVolume ();
		if (!isDetected) {
			if (currentVolume > volumeThreshold) {
				Debug.Log ("Start: " +currentVolume);
				isDetected = true;
				startTime = Time.time;
                debug_duration = 0;

                if(eventRecordAudioStart != null){
                    eventRecordAudioStart(startTime);
                }
			}

            // timeOut --
            float detectingTime = Time.time - detectStartTime;
            if (detectingTime > detectTimeout)
            {
                if (eventRecordAudioTimeout != null){
                    eventRecordAudioTimeout();
                }
                isDetected = false;
            }
		}

		if (isDetected) {
            float duration = Time.time - startTime;
            debug_duration = duration;
            if (duration > recMinTime && (currentVolume < volumeThreshold * 0.5f || duration > recLimit) ) {
                // EndRecord
                // 録音時間が最低時間以上で閾値以下または録音時間上限の時は終了
                isDetected = false;
				
				Debug.Log ("End: " + duration);
                if (eventRecordAudioComplete != null)
                {
                    eventRecordAudioComplete(duration + 1);
                }
			}
		}
	}

    [SerializeField] public float debug_duration;

	//void StopRecord()
	//{
	//	audioRecorder.StopRecord ();
	//}

    //[SerializeField] public float delayTime = 1.0f;

	//void PlayRecordData(float time)
	//{
 //       List<float[]> recordAudioData = audioRecorder.GetRecData();

 //       int range = ((int)time+1) * 43;
 //       int dataEnd = recordAudioData.Count;
 //       int dataCount = Mathf.Min(dataEnd, range);
 //       int startIndex = dataEnd - dataCount;
	//	List<float[]> recData = recordAudioData.GetRange(startIndex, dataCount);
	//}

    //public void StartReplay(List<float[]> recData)
    //{
    //    delayPlay0.SetRecordData(recData);
    //    delayPlay0.StartPlay();
    //}

    public void StartDetect()
    {
        detectStartTime = Time.time;

        enableDetect = true;
    }

    public void StopDetect()
    {
        enableDetect = false;
        //StopRecord();
    }

    public bool IsDetected()
    {
        return isDetected;
    }

    public bool IsEnableDetect()
    {
        return enableDetect;
    }
}
