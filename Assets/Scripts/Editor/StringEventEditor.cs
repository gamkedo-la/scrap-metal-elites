// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
//
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StringEvent))]
public class StringEventEditor : Editor
{
    public string stringToEdit = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;
        stringToEdit = GUILayout.TextArea(stringToEdit, 200);
        StringEvent e = target as StringEvent;
        if (GUILayout.Button("Raise")) {
            Debug.Log("stringToEdit: " + stringToEdit);
            e.Raise(stringToEdit);
        }
    }
}
