using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomTest))]
public class RandomTestCE : Editor
{
    RandomTest targetScript;

    void OnEnable()
    {
        targetScript = (RandomTest)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Rangom")) targetScript.RandomRange();

    }
}