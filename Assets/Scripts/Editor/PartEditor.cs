using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(Part))]
[CustomEditor(typeof(Part)), CanEditMultipleObjects]
public class PartEditor : Editor {

    SerializedProperty modelsProp;
    SerializedProperty applicatorsProp;
    SerializedProperty connectedPartsProp;
    protected EditorModelDisplayer displayer;

    void Awake() {
        displayer = new EditorModelDisplayer();
    }

    protected virtual void DisplayModel() {
        if (displayer == null) return;
        displayer.Clear();
        // do not display prefab models if application is playing
        if (Application.isPlaying) return;

        // display part
        for (var i=0; i<targets.Length; i++) {
            ((Part) targets[i]).Display(displayer);
        }
    }

    protected virtual void ClearModel() {
        if (displayer == null) return;
        displayer.Clear();
    }

    protected virtual void OnEnable() {
        // Setup the SerializedProperties.
        modelsProp = serializedObject.FindProperty("models");
        applicatorsProp = serializedObject.FindProperty("applicators");
        connectedPartsProp = serializedObject.FindProperty("connectedParts");
        DisplayModel();
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(modelsProp, true);
        EditorGUILayout.PropertyField(applicatorsProp, true);
        EditorGUILayout.PropertyField(connectedPartsProp, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

    protected virtual void OnDisable() {
        ClearModel();
    }
}
