using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://qiita.com/segur/items/6d7b140418ef9adf6e7c

[RequireComponent(typeof(AudioSource))]
public class RecordVoice : MonoBehaviour
{
	public int minDuration;                  //最小の録音時間（2秒とか）
	public int maxDuration;                  //最大の録音時間（20秒とか）
	public AudioClip audioClip;              //音声データ

	private const int sampleRate = 16000;    //録音のサンプリングレート
	private string mic;                      //マイクのデバイス名

	[SerializeField] AudioSource audioSrc;

	void Start()
	{
		Debug.Log (AudioSettings.outputSampleRate);

		string[] devices = Microphone.devices;
		for (int i = 0; i < devices.Length; i++) {
			Debug.Log (i + "/ "+devices[i]);
		}

		audioSrc = this.GetComponent<AudioSource> ();

		recordAudioData = new List<float[]>();
	}

	///-----------------------------------------------------------
	/// <summary>録音開始</summary>
	///-----------------------------------------------------------
	public void StartMicrophone( System.Action OnMicReadyAct = null)
	{
		//マイク存在確認
		if (Microphone.devices.Length == 0)
		{
			Debug.Log("マイクが見つかりません");
			return;
		}

		//マイク名
		mic = Microphone.devices[0];

		var sampleRate = AudioSettings.outputSampleRate;
		//録音開始。audioClipに音声データを格納。
		audioClip = Microphone.Start(mic, true, 5, sampleRate);

		// Wait until the microphone gets initialized.
		int delay = 0;
		while (delay <= 0) {
			delay = Microphone.GetPosition (null);
		}

		//
		audioSrc.clip = audioClip;
		audioSrc.loop = true;
		audioSrc.Play();

		// play
		if(OnMicReadyAct != null){
			OnMicReadyAct ();
		}
	}

	///-----------------------------------------------------------
	/// <summary>録音終了</summary>
	///-----------------------------------------------------------
	public void StopMic()
	{
		//マイクの録音を強制的に終了
		Microphone.End(mic);
	}

	public void StartRecord()
	{
		enableRecord = true;
	}
	public void StopRecord()
	{
		enableRecord = false;
	}
	public bool IsRecording()
	{
		return enableRecord;
	}


	public List<float[]> recordAudioData = new List<float[]>();
	public bool enableRecord = true;

	[SerializeField] public int debug_count = 0;
	[SerializeField] int dataBuffer = 500;

	void OnAudioFilterRead(float[] data, int channels) {

		if (!enableRecord) {
            // 出力をミュートする
            for (int i = 0; i < data.Length; i++){
                data[i] = 0;
            }
			return;
		}
		// 録音
		float[] copy_data = new float[data.Length]; // copyでやらないとうまくいかない
		System.Array.Copy(data, copy_data, data.Length);
		recordAudioData.Add (copy_data);

		debug_count = recordAudioData.Count;

		// 出力をミュートする
		for (int i = 0; i < data.Length; i++) {
			data[i] = 0;
		}

		// buffer以上貯めない
		if (debug_count > dataBuffer) {
			recordAudioData.RemoveAt (0);
		}
	}

	public void ClearRecordData()
	{
		recordAudioData.Clear ();
		debug_count = recordAudioData.Count;
	}

	// https://qiita.com/TyounanMOTI/items/5306336823316b0b289c
	// Unityでのマイク録音３種盛り：レイテンシ比較

	// http://akiiro.hatenablog.com/entry/2017/08/09/031237
	// 【Unity】AudioListerを録音してwavにする

	// http://soysoftware.sakura.ne.jp/archives/654
	// Unityボイスチェンジャーフィルタをアセットストアで公開しました


    public int Get_dataBuffer(){
        return dataBuffer;
    }
}