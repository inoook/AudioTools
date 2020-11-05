using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInputTest : MonoBehaviour
{
    [SerializeField] MicInput micInput;

    [SerializeField] Rect drawRect = new Rect(10,10,150,100);

    private void OnGUI() {
        GUILayout.BeginArea(drawRect);
        if (GUILayout.Button("MicOutput: true")) {
            micInput.EnableMicOutput(true);
        }
        if (GUILayout.Button("MicOutput: false")) {
            micInput.EnableMicOutput(false);
        }
        GUILayout.EndArea();
    }
}
