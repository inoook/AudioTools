using UnityEngine;
using System.Collections;

/// <summary>
/// 音階の表示
/// </summary>
public class DrawScaleWave : MonoBehaviour {

	[SerializeField] AudioAnalyzer audioAnalyzer = null;
	[SerializeField] DrawHistory drawHistory = null;

	// Update is called once per frame
	void Update () {
		DrawScaleByHertz( audioAnalyzer.GetPitchHertz () );
	}

	void DrawScaleByHertz(float hertz)
	{
		float scale = SoundLibrary.ConvertHertzToScale(hertz);
		DrawScale(scale);
	}
	void DrawScale(float scale)
	{
		// draw ------------
		drawHistory.AddValue (scale);
	}

}
