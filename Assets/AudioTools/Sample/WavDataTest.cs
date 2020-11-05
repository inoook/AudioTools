using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavDataTest : MonoBehaviour {

	[SerializeField] string file = "doremi001o.wav";

	WavData wavData = new WavData();

	// Use this for initialization
	void Start () {
		//string file = "doremi001o.wav";
		string path = Application.dataPath + "/AudioTools/Sample/sound_sample/" + file;
		wavData.ReadWave (path);
	}

	[SerializeField] float delta = 0.1f;
	[SerializeField] float amp = 0.1f;
	[SerializeField] Vector3 offset = Vector3.zero;
    private void Update()
    {
		//Debug.LogWarning(sound.ValueR.Length);
		for(int i = 0; i < wavData.ValueR.Length; i++)
        {
			Vector3 pos = new Vector3(i * delta, 0, 0) + offset;
			Vector3 dir = new Vector3(0, wavData.ValueR[i] * amp, 0);
			MeshLine.DrawRay(pos, dir, Color.red);
			//Debug.DrawRay(pos, dir);
        }
	}

	[SerializeField] Rect drawRect = new Rect(10,10,200,200);

	private void OnGUI()
    {
		GUILayout.BeginArea(drawRect);
		GUILayout.Label("Channel: "+wavData.WaveHeader.Channel);
		GUILayout.Label("PlayTimeMsec: "+wavData.WaveHeader.PlayTimeMsec);
		GUILayout.EndArea();
	}

}
