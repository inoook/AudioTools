using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerHelperTest : MonoBehaviour {


    [SerializeField] AudioMixerHelper audioMixerHelper;

    [SerializeField, Range(0, 1)] float volume = 1;


	// Update is called once per frame
	void Update () {
        audioMixerHelper.masterVolume = volume;
	}

    [SerializeField] AudioMixerHelper.SnapshotAndWeight[] snapshotAndWeights;
    [SerializeField] Rect drawRect = new Rect(10, 10, 200, 200);

    [SerializeField] AudioMixer mixer = null;

    private void OnGUI()
	{
        GUILayout.BeginArea(drawRect);
        if (GUILayout.Button("Change"))
        {
            AudioMixerHelper.Change(mixer, snapshotAndWeights, 5);
        }

        if (GUILayout.Button("FadeTo1"))
        {
            AudioMixerHelper.FadeTo(mixer, snapshotAndWeights[0].snapshot, 5);
        }
        if (GUILayout.Button("FadeTo2"))
        {
            AudioMixerHelper.FadeTo(mixer, snapshotAndWeights[1].snapshot, 5);
        }
        if (GUILayout.Button("FadeTo3"))
        {
            AudioMixerHelper.FadeTo(mixer, snapshotAndWeights[2].snapshot, 5);
        }
        GUILayout.EndArea();
	}
}
