using UnityEngine;
using System.Collections;

public class AudioAnalyzerDebug : MonoBehaviour {

	[SerializeField] AudioAnalyzer audioSpec = null;

	[SerializeField] float pitchMax = 0;
	[SerializeField] float volumeMax = 0;

	[SerializeField] Rect drawRect0 = new Rect (340, 10, 300, 300);

	void OnGUI()
	{
		GUILayout.BeginArea (drawRect0);
		GUILayout.Label(this.gameObject.name + " (AudioAnalyzerDebug)");
		//
		float pitch = audioSpec.GetPitchHertz();
		string codeStr = audioSpec.GetCode();

		pitchMax = Mathf.Max (pitch, pitchMax);
		GUILayout.Label ("pitch: "+pitch.ToString("0000.000") + " / max: "+pitchMax.ToString("0000.000") + " / code: "+codeStr);
		GUILayout.HorizontalSlider (pitch, 0, 3000);
		//
		float rmsValue = audioSpec.GetRmsValue();
		GUILayout.Label ("rmsValue: "+rmsValue);
		GUILayout.HorizontalSlider (rmsValue, 0, 0.5f);

		float dbLevel = audioSpec.GetDbLevel();
		GUILayout.Label ("Db: "+dbLevel);
		GUILayout.HorizontalSlider (dbLevel, -256, 256);

		//
		float volume = audioSpec.GetVolume();
		volumeMax = Mathf.Max (volume, volumeMax);
		GUILayout.Label ("volume: "+volume.ToString("0000.000") + " / max: "+volumeMax.ToString("0000.000"));
		GUILayout.HorizontalSlider (volume, 0, 0.5f);

		GUILayout.EndArea ();

	}
}
