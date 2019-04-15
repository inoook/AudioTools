using UnityEngine;
using System.Collections;

// http://tips.hecomi.com/entry/2014/11/11/021147
public class SpectrumAnalyzer : MonoBehaviour 
{
	[SerializeField]
	private AudioSource audioSrc;

	public int resolution = 1024;
//	public Transform lowMeter, midMeter, highMeter;
	public float lowFreqThreshold = 14700, midFreqThreshold = 29400, highFreqThreshold = 44100;
	public float lowEnhance = 1f, midEnhance = 10f, highEnhance = 100f;

	float low = 0f, mid = 0f, high = 0f;


	void Start()
	{
		
	}

	void Update() {
		float[] spectrum = new float[resolution];
		audioSrc.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

		var deltaFreq = AudioSettings.outputSampleRate / resolution;
		

		low = 0f;
		mid = 0f;
		high = 0f;

		for (var i = 0; i < resolution; ++i) {
			var freq = deltaFreq * i;
			if      (freq <= lowFreqThreshold)  low  += spectrum[i];
			else if (freq <= midFreqThreshold)  mid  += spectrum[i];
			else if (freq <= highFreqThreshold) high += spectrum[i];
		}

		low  *= lowEnhance;
		mid  *= midEnhance;
		high *= highEnhance;
//
//		lowMeter.localScale  = new Vector3(lowMeter.localScale.x,  low,  lowMeter.localScale.z);
//		midMeter.localScale  = new Vector3(midMeter.localScale.x,  mid,  midMeter.localScale.z);
//		highMeter.localScale = new Vector3(highMeter.localScale.x, high, highMeter.localScale.z);
	}

	[SerializeField]
	Rect drawRect = new Rect(10,10,200,200);
	[SerializeField]
	float max = 1.0f;
//
//	void OnGUI()
//	{
//		GUILayout.BeginArea (drawRect);
//		GUILayout.HorizontalSlider (low, 0, max);
//		GUILayout.HorizontalSlider (mid, 0, max);
//		GUILayout.HorizontalSlider (high, 0, max);
//		GUILayout.EndArea ();
//	}
//
	void OnGUI()
	{
		GUILayout.BeginArea (drawRect);
		GUILayout.Label ("SpectrumAnalyzer");
		GUILayout.BeginHorizontal ();

		GUILayout.VerticalSlider (low, max, 0);
		GUILayout.VerticalSlider (mid, max, 0);
		GUILayout.VerticalSlider (high, max, 0);

		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}
}