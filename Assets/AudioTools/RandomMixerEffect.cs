using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

public class RandomMixerEffect : MonoBehaviour {

    [SerializeField] AudioSource audioSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    void SetMixer(AudioMixerGroup mixerGroup)
    {
        audioSource.outputAudioMixerGroup = mixerGroup;
    }

    [SerializeField] AudioMixerGroup[] audioMixerGroups;
    [SerializeField] Rect drawRect = new Rect(10,10,200,200);

	private void OnGUI()
	{
        GUILayout.BeginArea(drawRect);
        for (int i = 0; i < audioMixerGroups.Length; i++){
            AudioMixerGroup g = audioMixerGroups[i];
            if(GUILayout.Button(g.name)){
                SetMixer(g);
            }
        }
        GUILayout.EndArea();
	}
}
