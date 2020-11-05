// http://ibako-study.hateblo.jp/entry/2014/02/06/225256
using UnityEngine;
using System.Collections;

public class MicroFFT : MonoBehaviour {

	[SerializeField]
	FFTWindow fftWindow = FFTWindow.BlackmanHarris;

	// マイクからの音を拾う
	public AudioSource audioSrc;
//	private string mic_name = "UAB-80";

	// 波形を描画する
	public LineRenderer line;

	// 波形描画のための変数
	private float[] wave;
	private int wave_num;
	private int wave_count;

	[SerializeField]
	public string currentS;


	void Start () {
		// 波形描画のための変数の初期化
		wave_num = 800;
		wave = new float[wave_num];
		wave_count = 0;
	}

	void Update () {
		// 諸々の解析
		float hertz = SoundLibrary.AnalyzeSound(audioSrc, 1024, 0.02f, fftWindow);// pitch
		float scale = SoundLibrary.ConvertHertzToScale(hertz);
		string s = SoundLibrary.ConvertScaleToString(scale);
		Debug.Log(s + " / "+hertz + "Hz, Scale:" + scale);
		currentS = s;

		// draw ------------
		// 波形描画
		wave[wave_count] = scale;
		SoundLibrary.ScaleWave(wave, wave_count, wave_num, line);
		wave_count++;
		if (wave_count >= wave_num) wave_count = 0;
	}
}