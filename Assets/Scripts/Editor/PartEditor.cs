using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(Part))]
[CustomEditor(typeof(Part)), CanEditMultipleObjects]
public class PartEditor : Editor {

    SerializedProperty massProp;
    SerializedProperty dragProp;
    SerializedProperty angularDragProp;
    SerializedProperty modelsProp;
    SerializedProperty applicatorsProp;
    SerializedProperty connectedPartsProp;
    List<GameObject> displayedGos = null;

    void Awake() {
        displayedGos = new List<GameObject>();
    }

    protected virtual void DisplayModel() {
        if (displayedGos != null) {
            ClearModel();
        }
        // don't display if application is playing
        if (Application.isPlaying) return;
        // build display config to hide built objects and not save
        var displayConfig = new PartConfig();
        displayConfig.Save<bool>(PartUtil.hideTag, true);
        displayConfig.Save<bool>(PartUtil.dontsaveTag, true);

        // build the part
        for (var i=0; i<targets.Length; i++) {
            displayedGos.Add(((Part) targets[i]).Build(displayConfig, null, "display"));
        }
    }

    protected virtual void ClearModel() {
        if (displayedGos != null) {
            for (var i=0; i<displayedGos.Count; i++) {
                DestroyImmediate(displayedGos[i]);
                displayedGos[i] = null;
            }
            displayedGos = new List<GameObject>();
        }

    }

    protected virtual void OnEnable() {
        // Setup the SerializedProperties.
        massProp = serializedObject.FindProperty("mass");
        dragProp = serializedObject.FindProperty("drag");
        angularDragProp = serializedObject.FindProperty("angularDrag");
        modelsProp = serializedObject.FindProperty("models");
        applicatorsProp = serializedObject.FindProperty("applicators");
        connectedPartsProp = serializedObject.FindProperty("connectedParts");
        DisplayModel();
    }

    public override void OnInspectorGUI() {
        if (Application.isPlaying && displayedGos != null) {
            ClearModel();
        }
        serializedObject.Update();
        EditorGUILayout.PropertyField(massProp);
        EditorGUILayout.PropertyField(dragProp);
        EditorGUILayout.PropertyField(angularDragProp);
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
