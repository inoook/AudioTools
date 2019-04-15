using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInput_throughOut_test : MonoBehaviour
{
    [SerializeField] MicInput micInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }

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
