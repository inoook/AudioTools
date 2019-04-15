using UnityEngine;
using System.Collections;

public class VJkitAudioAnalize : MonoBehaviour {

	[SerializeField]
	AudioSource	audioSrc;

	[SerializeField]
	FFTWindow fftWindow = FFTWindow.Rectangular;

	public float _scaleX = 1.0f;
	public float _scaleY = 1.0f;

	enum BAND_TYPE
	{
		BAND_4, BAND_8, BAND_10, BAND_31
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAnalize ();
	}

	//---------------
	// Octave band frequency.
	static float[][] middleFrequenciesForBands = {
		new float[]{ 125.0f, 500, 1000, 2000 },
		new float[]{ 63.0f, 125, 500, 1000, 2000, 4000, 6000, 8000 },
		new float[]{ 31.5f, 63, 125, 250, 500, 1000, 2000, 4000, 8000, 16000 },
		new float[]{ 20.0f, 25, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000 }
	};
	// Octave band width.
	static float[] bandwidthForBands = {
		1.414f, // 2^(1/2)
		1.414f, // 2^(1/2)
		1.414f, // 2^(1/2)
		1.122f  // 2^(1/6)
	};
	// FFT point number settings.
	static int[] fftPointNumberForBands = { 1024, 2048, 2048, 4096 };

	float[] bandLevels;

	[SerializeField]
	BAND_TYPE band = BAND_TYPE.BAND_31;
	int bandType = 3;

	int sampleRate;
	float[] frequencies;
	float bandwidth;

	float[] fftSpectrum;
	int FrequencyToFftIndex (float f, float sampleRate)
	{
		var points = fftSpectrum.Length;
		var index = Mathf.FloorToInt (f / sampleRate * 2.0f * points);
		return Mathf.Clamp (index, 0, points - 1);
	}

	void UpdateAnalize()
	{
		bandType = (int)band;

		int fftNumber = fftPointNumberForBands [(int)bandType];
		if (fftSpectrum == null || fftSpectrum.Length * 2 != fftNumber)
		{
			fftSpectrum = new float[fftNumber / 2];
		}

		audioSrc.GetSpectrumData( fftSpectrum, 0, fftWindow );

		sampleRate = AudioSettings.outputSampleRate;
		frequencies = middleFrequenciesForBands [bandType];
		bandwidth = bandwidthForBands [bandType];


		var bandCount = middleFrequenciesForBands [(int)bandType].Length;
		if (bandLevels == null || bandLevels.Length != bandCount)
		{
			bandLevels = new float[bandCount];
		}

		for (var bi = 0; bi < bandCount; bi++)
		{
			// Specify the spectrum range of the band.
			int imin = FrequencyToFftIndex (frequencies [bi] / bandwidth, sampleRate);
			int imax = FrequencyToFftIndex (frequencies [bi] * bandwidth, sampleRate);

			// Specify the max level of the band.
			var bandMax = fftSpectrum [imin];
			for (var fi = imin + 1; fi < imax; fi++)
			{
				bandMax = Mathf.Max (bandMax, fftSpectrum [fi]);
			}

			// Convert amplitude to decibel.
			bandMax = 20.0f * Mathf.Log10 (bandMax * 2 + 1.5849e-13f);

			// Store the result.
			bandLevels [bi] = bandMax;
		}

		// draw
		for (int i = 1; i < bandLevels.Length; i++) {
			Debug.DrawLine (new Vector3 ((i - 1) * _scaleX, bandLevels [i - 1] * _scaleY, 0), new Vector3 (i * _scaleX, bandLevels [i] * _scaleY, 0), Color.yellow);
		}			
	}
}
