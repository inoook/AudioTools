using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// AudioMixer を使い勝手よくする為の helper
/// </summary>
public class AudioMixerHelper : MonoBehaviour {

    // http://tsubakit1.hateblo.jp/entry/2015/05/23/234053

    [SerializeField] AudioMixer mixer = null;

    public float masterVolume
    {
        set { 
            mixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, value));
        }
    }

    [System.Serializable]
    public class SnapshotAndWeight
    {
        public AudioMixerSnapshot snapshot;
        public float weight;
    }

    /// <summary>
    /// SnapshotAndWeight で指定した snapshot へ audioMixer を変更（weightを含め）する。
    /// </summary>
    /// <param name="mixer"></param>
    /// <param name="snapshotAndWeights"></param>
    /// <param name="time"></param>
    public static void Change(AudioMixer mixer, SnapshotAndWeight[] snapshotAndWeights, float time)
    {
        int count = snapshotAndWeights.Length;
        AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[count];
        float[] weights = new float[count];
        for (int i = 0; i < snapshotAndWeights.Length; i++){
            SnapshotAndWeight sw = snapshotAndWeights[i];
            snapshots[i] = sw.snapshot;
            weights[i] = sw.weight;
        }
        mixer.TransitionToSnapshots(snapshots, weights, time);
    }

    public static void Change(AudioMixer mixer, AudioMixerSnapshot[] snapshots, float[] weights, float time)
    {
        mixer.TransitionToSnapshots(snapshots, weights, time);
    }

    /// <summary>
    /// mixer を from から to の Snapshot へ指定時間かけて遷移させる。
    /// </summary>
    /// <param name="mixer"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    /// <param name="toWeight"></param>
    public static void FadeTo(AudioMixer mixer, AudioMixerSnapshot from, AudioMixerSnapshot to, float time, float toWeight = 1.0f)
    {
        AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[2];
        snapshots[0] = from;
        snapshots[1] = to;
        float[] weights = new float[]{0, toWeight};
        mixer.TransitionToSnapshots(snapshots, weights, time);
    }

    public static void FadeTo(AudioMixer mixer, AudioMixerSnapshot to, float time, float toWeight = 1.0f)
    {
        AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[2];
        snapshots[0] = to;// TODO: 新しいバージョンでエラーが起きないか注意
        snapshots[1] = to;
        float[] weights = new float[] { 0, toWeight };
        mixer.TransitionToSnapshots(snapshots, weights, time);
    }
}
