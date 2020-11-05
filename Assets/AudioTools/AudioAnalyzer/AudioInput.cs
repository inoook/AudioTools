using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioInput : MonoBehaviour {

	static AudioInput INSTANCE;
	
	[SerializeField] AudioAnalyzer analyzer;

	[SerializeField] DrawSpectrumHertzArea spectrumHertzArea;

	List<float> avgPitchList = new List<float>();
	[SerializeField] int avgPitchBufferNum = 10;
	[SerializeField] float avgPitch = 0;

	[Header("Pitch Axis setting")]
	[SerializeField] float pitchMin = 200;
	[SerializeField] float pitchBase = 400;
	[SerializeField] float pitchMax = 3000;

	[SerializeField] float volumeThreshold = 0.02f; // volume が volumeThreshold 以下の時、pitchBase（センター位置）に

	[Header("Pitch01 setting")]
	[SerializeField] float smoothPitch = 0;
	[SerializeField] float pitch01Max = 3000;

	[SerializeField, Range(-1, 1)] float pitchAxis = 0;
	[SerializeField, Range(0, 1)] float pitch01 = 0;

	//
	[Header("Volume01 setting")]
	[SerializeField] float smoothVolume = 0;
	[SerializeField] float volumeMax = 0.4f;
	[SerializeField, Range(0,1)] float volume01 = 0;


	// Use this for initialization
	void Start () {
		INSTANCE = this;
	}
	
	// Update is called once per frame
	void Update () {
		
		float pitch = analyzer.GetPitchHertz();
		float volume = analyzer.GetVolume();
		
		smoothPitch = Mathf.Lerp(smoothPitch, pitch, 0.1f);

		float t_pitch01 = Remap (smoothPitch, 0, pitch01Max, 0, 1.0f);
		pitch01 = Mathf.Clamp01 (t_pitch01);

		// ----------
		// pitch
		if (volume < volumeThreshold) {
			pitch = pitchBase;
		}

		avgPitchList.Add (pitch);
		if (avgPitchList.Count > avgPitchBufferNum) {
			avgPitchList.RemoveRange(0, avgPitchList.Count - avgPitchBufferNum);
		}
		avgPitch = CalucAvg (avgPitchList);

		// ----------
		// pitchAxis
		float t_pitch = avgPitch;
		if (t_pitch > pitchBase) {
			t_pitch = Remap (t_pitch, pitchBase, pitchMax, 0, 1.0f);
		}else{
			t_pitch = Remap (t_pitch, pitchMin, pitchBase, -1.0f, 0);
		}
		t_pitch = Mathf.Clamp (t_pitch, -1.0f, 1.0f);
		pitchAxis = t_pitch;

		// ----------
		// volume
		smoothVolume = Mathf.Lerp(smoothVolume, volume, 0.1f);

		// volume01
		float t_volume = Remap (smoothVolume, 0, volumeMax, 0, 1.0f);
		volume01 = Mathf.Clamp01 (t_volume);
	}


	public float GetRawAreaValue(int index)
	{
		return spectrumHertzArea.GetRawAreaValueByIndex(index);
	}
	public float GetAreaValue01(int index, float amp)
	{
		return spectrumHertzArea.GetAreaValue01ByIndex(index, amp);
	}
	public int _GetAreaCount()
	{
		return spectrumHertzArea.GetAreaCount();
	}
	public DrawSpectrumHertzArea.HertzArea _GetHertzAreaByIndex(int index)
	{
		return spectrumHertzArea.GetHertzAreaByIndex (index);
	}

	public float GetVolume01()
	{
		return volume01;
	}
	public float GetPitch01()
	{
		return pitch01;
	}


	#region static API
	public static float GetInputPitchAxis()
	{
		return INSTANCE.pitchAxis;
	}
	public static float GetInputPitch01()
	{
		return INSTANCE.pitch01;
	}
	public static float GetInputVolume01()
	{
		return INSTANCE.volume01;
	}
	public static int GetAreaCount()
	{
		return INSTANCE._GetAreaCount();
	}
	public static float GetRawInputHertzArea(int index)
	{
		return INSTANCE.GetRawAreaValue(index);
	}
	public static float GetInputValue01HertzArea(int index, float amp = 1.0f)
	{
		return INSTANCE.GetAreaValue01(index, amp);
	}

	public static DrawSpectrumHertzArea.HertzArea GetHertzAreaByIndex(int index)
	{
		return INSTANCE._GetHertzAreaByIndex(index);
	}
	#endregion

	#region static Utils
	// utils
	public static float CalucAvg(List<float> values)
	{
		float v = 0;
		for (int i = 0; i < values.Count; i++) {
			v += values[i];
		}
		return v / values.Count;
	}

	public static float Remap (float value, float inputMin, float inputMax, float outputMin, float outputMax) {
		return (value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
	}
	#endregion
}
