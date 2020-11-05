using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInputDebug : MonoBehaviour
{

	[SerializeField] Rect drawRect = new Rect(10, 10, 300, 300);

	private void OnGUI()
    {
		GUILayout.BeginArea(drawRect);

		float pitchAxis = AudioInput.GetInputPitchAxis();
		float pitch01 = AudioInput.GetInputPitch01();
		float volume01 = AudioInput.GetInputVolume01();

		GUILayout.Label("pitchAxis: " + pitchAxis.ToString("0.000"));
		GUILayout.HorizontalSlider(pitchAxis, -1, 1);

		GUILayout.Label("pitch01: " + pitch01.ToString("0.000"));
		GUILayout.HorizontalSlider(pitch01, 0, 1);

		GUILayout.Label("volume01: " + volume01.ToString("0.000"));
		GUILayout.HorizontalSlider(volume01, 0, 1);

		GUILayout.Space(20);
		int areaCount = AudioInput.GetAreaCount();
		GUILayout.Label("HertzArea: " + areaCount);
		for (int i = 0; i < areaCount; i++)
		{
			DrawSpectrumHertzArea.HertzArea area = AudioInput.GetHertzAreaByIndex(i);

			float areaRawValue = AudioInput.GetRawInputHertzArea(i); // area 番号に含まれる出力を取得
			float areaValue = AudioInput.GetInputValue01HertzArea(i, 2.0f);
			GUILayout.Label(area.label + ": " + areaRawValue.ToString("0.000") + "  " + area.value_max.ToString("0.000"));
			GUILayout.HorizontalSlider(areaRawValue, 0, 1);
			GUILayout.HorizontalSlider(areaValue, 0, 1);

		}
		GUILayout.EndArea();
	}
}
