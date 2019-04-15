using UnityEngine;
using System.Collections;
using System.Linq;

// http://www.ab.auone-net.jp/~eguitar/Sin_Waves.html
// http://www.cs.miyazaki-u.ac.jp/~date/lectures/am2/kadai/kadai0am2.html
// http://tips.hecomi.com/entry/2014/11/11/021147
// http://answers.unity3d.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
// 
//[RequireComponent (typeof(AudioSource))]
public class AudioAnalyzer : MonoBehaviour
{

	[SerializeField]
	AudioSource audioSrc;

	[SerializeField]
	int waveDataLength = 256;
	[SerializeField]
	int spectrumLength = 2048;


	[SerializeField]
	FFTWindow fftWindow = FFTWindow.Rectangular;

	void Awake()
	{
		if (audioSrc == null) {
			audioSrc = this.gameObject.GetComponent<AudioSource> ();
		}
	}

	
	public float volume;
	public float pitch;
	public float dbLevel;
	public float rmsValue;
	public string codeStr;


	#region API
	float[] waveData;
	public float[] GetWaveData()
	{
		return waveData;
	}

	float[] spectrum;
	public float[] GetSpectrum()
	{
		return spectrum;
	}

	public float GetPitchHertz()
	{
		return pitch;
	}
	public float GetVolume()
	{
		return volume;
	}
	#endregion

	
	void Update( )
	{
		//#####################
		// 波形
//		float[] waveData = new float[waveDataLength];
		waveData = new float[waveDataLength];
		audioSrc.GetOutputData (waveData, 0);

		// volume
		volume = GetAveragedVolume (waveData);
		//

		// db
		dbLevel = GetDbLevel (waveData);


		//#####################
		// スペクトラム
		// FFT: x軸が周波数・y軸が分布に変えること
//		float[] spectrum = new float[spectrumLength];
		spectrum = new float[spectrumLength];
		audioSrc.GetSpectrumData( spectrum, 0, fftWindow );

		//-------------------
		// pitch
		pitch = GetPitchValue(spectrum);

		//--------------------
		// code
		float scale = SoundLibrary.ConvertHertzToScale(pitch);
		codeStr = SoundLibrary.ConvertScaleToString(scale);
	}

	//
	float GetAveragedVolume(float[] waveData)
	{ 
		float a = 0;
		foreach(float s in waveData)
		{
			a += Mathf.Abs(s);
		}
		return a/(float)waveData.Length;
	}

	//
	float GetDbLevel(float[] waveData)
	{
		const float zeroOffset = 1.5849e-13f;
		const float refLevel = 0.70710678118f; // 1/sqrt(2)
		float squareSum = waveData.Select(x => x*x).Sum();
		float sampleCount = waveData.Length / 1;

		float rms = Mathf.Min(1.0f, Mathf.Sqrt(squareSum / sampleCount));

		rmsValue = rms;
		return 20.0f * Mathf.Log10(rms / refLevel + zeroOffset);
	}

	// http://answers.unity3d.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
	float GetPitchValue(float[] spectrum)
	{
		int qSamples = spectrum.Length;  // array size
		float fSample = AudioSettings.outputSampleRate;

		float threshold = 0.02f;      // minimum amplitude to extract pitch

		float maxV = 0;
		int maxN = 0;
		for (int i=0; i < qSamples; i++){ // find max 
			if (spectrum[i] > maxV && spectrum[i] > threshold){
				maxV = spectrum[i];
				maxN = i; // maxN is the index of max
			}
		}

		float freqN = maxN; // pass the index to a float variable
		if (maxN > 0 && maxN < qSamples-1){ // interpolate index using neighbours
			var dL = spectrum[maxN-1]/spectrum[maxN];
			var dR = spectrum[maxN+1]/spectrum[maxN];
			freqN += 0.5f*(dR*dR - dL*dL);
		}
		float pitchValue = freqN*(fSample/2.0f)/qSamples; // convert index to frequency
		return pitchValue;
	}

}