using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUtil_test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoundUtils sound = new SoundUtils ();
		string file = "doremi001o.wav";
		string path = Application.dataPath + "/AudioTools/sound_sample/" + file;
		Debug.Log (path);
		sound.ReadWave (path);
	}
	
}
