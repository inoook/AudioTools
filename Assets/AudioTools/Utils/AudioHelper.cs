using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MasterVolue 変更
/// </summary>
public class AudioHelper : MonoBehaviour {

    [SerializeField, Range(0, 1)] float masterVolume = 1;
	
	// Update is called once per frame
	void Update () {
        AudioHelper.listenerVolume = masterVolume;
	}

    public static float listenerVolume
    {
        set {
            AudioListener.volume = value;
        }
    }
}
