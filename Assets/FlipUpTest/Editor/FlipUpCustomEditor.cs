using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlipUp))]
public class FlipUpCustomEditor : Editor
{

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Flip")) (target as FlipUp).Flip();
    }
}
