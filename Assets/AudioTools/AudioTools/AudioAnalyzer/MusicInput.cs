using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicInput : MonoBehaviour {

	static MusicInput INSTANCE;
	
	[SerializeField]
	AudioAnalyzer analyzer;

	[SerializeField]
	DrawSpectrumHertzArea spectrumHertzArea;

	//
	List<float> avgPitchList = new List<float>();
	//List<float> avgPitch01List = new List<float>();
	//List<float> avgVolumeList = new List<float>();

	//
	[Header("Pitch setting")]
	[SerializeField]
	int pitchBufferNum = 30;
	[SerializeField]
	float avgPitch = 0;

	[SerializeField]
	float pitchMin = 200;
	[SerializeField]
	float pitchMax = 3000;
	[SerializeField]
	float pitchBase = 400;

	[SerializeField]
	float volumeThreshold = 0.02f;

	[Header("Pitch01 setting")]
	[SerializeField] float avgPitch01 = 0;
	//[SerializeField] int pitch01BufferNum = 30;
	[SerializeField] float pitch01Max = 3000;


	//
	[Header("Volume01 setting")]
	//[SerializeField] int volumeBufferNum = 30;
	[SerializeField] float avgVolume = 0;

	[SerializeField]
	float volumeMax = 0.4f;


	// Use this for initialization
	void Start () {
		INSTANCE = this;
	}
	
	// Update is called once per frame
	void Update () {
		
		float pitch = analyzer.pitch;
		float volume = analyzer.volume;
		
		// pitch01
//		avgPitch01List.Add (pitch);
//		if (avgPitch01List.Count > pitch01BufferNum) {
//			avgPitch01List.RemoveRange(0, avgPitch01List.Count - pitch01BufferNum);
//		}
//		avgPitch01 = CalucAvg (avgPitch01List);

		pitchFilter.a = 0.9f;
		avgPitch01 = pitchFilter.GetFilteredValue (pitch);

		float t_pitch01 = Remap (avgPitch01, 0, pitch01Max, 0, 1.0f);
		pitch01 = Mathf.Clamp01 (t_pitch01);

		// pitch -------------
		if (volume < volumeThreshold) {
			pitch = pitchBase;
		}

		avgPitchList.Add (pitch);
		if (avgPitchList.Count > pitchBufferNum) {
			avgPitchList.RemoveRange(0, avgPitchList.Count - pitchBufferNum);
		}
		avgPitch = CalucAvg (avgPitchList);

		// pitchAxis
		float t_pitch = avgPitch;
		if (t_pitch > pitchBase) {
			t_pitch = Remap (t_pitch, pitchBase, pitchMax, 0, 1.0f);
		}else{
			t_pitch = Remap (t_pitch, pitchMin, pitchBase, -1.0f, 0);
		}
		t_pitch = Mathf.Clamp (t_pitch, -1.0f, 1.0f);
		pitchAxis = t_pitch;


		// volume -------------
//		avgVolumeList.Add (volume);
//		if (avgVolumeList.Count > volumeBufferNum) {
//			avgVolumeList.RemoveRange(0, avgVolumeList.Count - volumeBufferNum);
//		}
//		avgVolume = CalucAvg (avgVolumeList);

		volumeFilter.a = 0.9f;
		avgVolume = volumeFilter.GetFilteredValue (volume);

		// volume01
		float t_volume = Remap (avgVolume, 0, volumeMax, 0, 1.0f);
		volume01 = Mathf.Clamp01 (t_volume);

		// hertz area -----------------

	}

	RCFilter volumeFilter = new RCFilter();
	RCFilter pitchFilter = new RCFilter();

	public float GetRawAreaValue(int index)
	{
		return spectrumHertzArea.GetRawAreaValueByIndex(index);
	}
	public float GetAreaValue(int index, float amp)
	{
		return spectrumHertzArea.GetAreaValueByIndex(index, amp);
	}
	public int _GetAreaCount()
	{
		return spectrumHertzArea.GetAreaCount();
	}
	public DrawSpectrumHertzArea.HertzArea _GetHertzAreaByIndex(int index)
	{
		return spectrumHertzArea.GetHertzAreaByIndex (index);
	}


	[SerializeField] float pitchAxis;
	[SerializeField] float pitch01;
	[SerializeField] float volume01;

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
	public static float GetInputHertzArea(int index, float amp = 1.0f)
	{
		return INSTANCE.GetAreaValue(index, amp);
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
