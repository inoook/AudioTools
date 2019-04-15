using UnityEngine;
using System.Collections;

public class DrawVolume : MonoBehaviour {

	[SerializeField]
	AudioAnalyzer audioAnalyzer;

	[SerializeField]
	DrawHistory drawHistory;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		float v = audioAnalyzer.GetVolume ();
		drawHistory.AddValue (v);
	}

}
