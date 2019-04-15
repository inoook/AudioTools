using UnityEngine;
using System.Collections;


public class DebugDisplay : MonoBehaviour {

	[SerializeField]
	AudioAnalyzer audioSpec;

	[SerializeField]
	float pitchMax;

	[SerializeField]
	float volumeMax;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[SerializeField]
	MusicInput musicInput;

	[SerializeField]
	Rect drawRect0 = new Rect (340, 10, 300, 300);
	[SerializeField]
	Rect drawRect1 = new Rect (10, 10, 300, 300);

	[SerializeField] bool drawExtra = false;

	void OnGUI()
	{
		GUILayout.BeginArea (drawRect0);

		//
		float pitch = audioSpec.pitch;
		string codeStr = audioSpec.codeStr;

		pitchMax = Mathf.Max (pitch, pitchMax);
		GUILayout.Label ("pitch: "+pitch.ToString("0000.000") + " / max: "+pitchMax.ToString("0000.000") + " / code: "+codeStr);
		GUILayout.HorizontalSlider (pitch, 0, 3000);


		//
		float rmsValue = audioSpec.rmsValue;
		GUILayout.Label ("rmsValue: "+rmsValue);
		GUILayout.HorizontalSlider (rmsValue, 0, 0.5f);

		float dbLevel = audioSpec.dbLevel;
		GUILayout.Label ("Db: "+dbLevel);
		GUILayout.HorizontalSlider (dbLevel, -256, 256);

		//
		float volume = audioSpec.volume;
		volumeMax = Mathf.Max (volume, volumeMax);
		GUILayout.Label ("volume: "+volume.ToString("0000.000") + " / max: "+volumeMax.ToString("0000.000"));
		GUILayout.HorizontalSlider (volume, 0, 0.5f);

		//
		GUILayout.Label ("volume01: "+musicInput.GetVolume01().ToString("0000.000"));
		GUILayout.HorizontalSlider (musicInput.GetVolume01(), 0, 1);

		GUILayout.Label ("pitch01: "+musicInput.GetPitch01().ToString("0000.000"));
		GUILayout.HorizontalSlider (musicInput.GetPitch01(), 0, 1);

		GUILayout.EndArea ();

		if (drawExtra) {
			// use sample -----------------------
			GUILayout.BeginArea (drawRect1);

			float pitchAxis = MusicInput.GetInputPitchAxis ();
			float pitch01 = MusicInput.GetInputPitch01 ();
			float volume01 = MusicInput.GetInputVolume01 ();

			GUILayout.Label ("pitchAxis: " + pitchAxis.ToString ("0.000"));
			GUILayout.HorizontalSlider (pitchAxis, -1, 1);

			GUILayout.Label ("pitch01: " + pitch01.ToString ("0.000"));
			GUILayout.HorizontalSlider (pitch01, 0, 1);

			GUILayout.Label ("volume01: " + volume01.ToString ("0.000"));
			GUILayout.HorizontalSlider (volume01, 0, 1);

			GUILayout.Space (20);
			int areaCount = MusicInput.GetAreaCount ();
			GUILayout.Label ("HertzArea: " + areaCount);
			for (int i = 0; i < areaCount; i++) {
				DrawSpectrumHertzArea.HertzArea area = MusicInput.GetHertzAreaByIndex (i);

				float areaRawValue = MusicInput.GetRawInputHertzArea (i);
				float areaValue = MusicInput.GetInputHertzArea (i, 2.0f);
				GUILayout.Label (area.label + ": " + areaRawValue.ToString ("0.000") + "  " + area.value_max.ToString ("0.000"));
				GUILayout.HorizontalSlider (areaRawValue, 0, 1);
				GUILayout.HorizontalSlider (areaValue, 0, 1);

			}
			GUILayout.EndArea ();
		}
	}
}
