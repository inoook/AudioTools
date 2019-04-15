using UnityEngine;
using System.Collections;

public class DrawScaleWave : MonoBehaviour {

	[SerializeField]
	AudioAnalyzer audioAnalyzer;

	[SerializeField]
	DrawHistory drawHistory;

	// Use this for initialization
	void Start () {
	}

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
