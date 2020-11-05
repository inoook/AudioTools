using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://qiita.com/segur/items/6d7b140418ef9adf6e7c

// https://qiita.com/TyounanMOTI/items/5306336823316b0b289c
// Unityでのマイク録音３種盛り：レイテンシ比較

// http://akiiro.hatenablog.com/entry/2017/08/09/031237
// 【Unity】AudioListerを録音してwavにする

// http://soysoftware.sakura.ne.jp/archives/654
// Unityボイスチェンジャーフィルタをアセットストアで公開しました

[RequireComponent(typeof(AudioSource))]
public class AudioRecorder : MonoBehaviour
{
	public void StartRecord()
	{
		recordAudioData = new List<float[]>();
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


	List<float[]> recordAudioData = new List<float[]>();
	bool enableRecord = false;

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

	public List<float[]> GetRecData()
    {
		return recordAudioData;
	}

	public void ClearRecordData()
	{
		recordAudioData.Clear ();
		debug_count = recordAudioData.Count;

		enableRecord = false;
	}

    public int Get_dataBuffer(){
        return dataBuffer;
    }
}