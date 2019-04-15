using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpectrumCircle : MonoBehaviour {

	[SerializeField]
	AudioSource audioSrc;

	[SerializeField]
	int sampleNum = 1024;
	[SerializeField]
	FFTWindow fftWindow = FFTWindow.Hamming;

//	[SerializeField]
//	int fftAmp = 40;

	[SerializeField]
	float scaleAmp = 1;

	[SerializeField]
	GameObject prefab;

	[SerializeField]
	int numberOfObjects = 20;

	[SerializeField]
	float smoothSpeed = 30.0f;

	[SerializeField]
	float radius = 5.0f;

	List<Transform> transList;



	// Use this for initialization
	void Start () {

		transList = new List<Transform> ();

		float delta = Mathf.PI * 2 / numberOfObjects;
		for (int i = 0; i < numberOfObjects; i++) {
			float angle = delta * i;
			GameObject g = GameObject.Instantiate<GameObject>(prefab);
			transList.Add (g.transform);

			g.transform.SetParent (this.transform);
			Vector3 pos = new Vector3 (Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
			g.transform.localPosition = pos;
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		float[] samples = new float[sampleNum];
//
//		audioSrc.GetSpectrumData (samples, 0, fftWindow);
//		for (int i = 0; i < numberOfObjects; i++) {
//			Transform t = transList[i];
//			float v = samples[i * fftAmp];
//			Vector3 currentScale = t.localScale;
//			currentScale.y = Mathf.Lerp (currentScale.y, v * scaleAmp, Time.deltaTime * smoothSpeed);
//			t.localScale = currentScale;
//		}
//	}

	[SerializeField]
	int min = 0;
	[SerializeField]
	int max = 512;

	[SerializeField]
	bool useAverage = false;

	[SerializeField]
	AnimationCurve valueOffsetCurve;

	public float maxSampleValue;

	float[] values;

	void Update () {
		float[] samples = new float[sampleNum];
		audioSrc.GetSpectrumData (samples, 0, fftWindow);

		float[] temp_samples = new float[sampleNum+1];
		System.Array.Copy (samples, temp_samples, samples.Length);
		temp_samples [temp_samples.Length - 1] = maxSampleValue;
		maxSampleValue = Mathf.Max (temp_samples);

//		max = Mathf.Min (max, sampleNum);

		// 指定エリアのfftを表示
		min = Mathf.Clamp (min, 0, max);
		max = Mathf.Clamp (max, min, sampleNum);

		int d = max - min;
		int delta = d / numberOfObjects;
		

		values = new float[numberOfObjects];
		for (int i = 0; i < numberOfObjects; i++) {
			float v = 0;
			for (int n = min + delta * i; n < min + delta * (i+1); n++) {
				v += samples[n];
			}
			if(useAverage){
				v /= (float)delta;
				v *= 20;
			}

//			v = Mathf.Clamp (v, 0, maxSampleValue);

			// 前半の突出を調整するため
			v *= valueOffsetCurve.Evaluate ((float)i / (float)numberOfObjects);
			values [i] = v;

			// view
			Transform t = transList [i];
			Vector3 currentScale = t.localScale;
			currentScale.y = Mathf.Lerp (currentScale.y, v * scaleAmp, Time.deltaTime * smoothSpeed);
			t.localScale = currentScale;
		}
	}

	[SerializeField]
	Rect drawRect = new Rect(10,10,200,200);
	[SerializeField]
	float limit = 1.0f;

	void OnGUI()
	{
		GUILayout.BeginArea (drawRect);
		GUILayout.Label ("SpectrumCircle");
		GUILayout.BeginHorizontal ();
		for (int i = 0; i < values.Length; i++) {
			float v = values[i];
			GUILayout.VerticalSlider (v, limit, 0);
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}
}
