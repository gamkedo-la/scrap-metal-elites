using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(Part))]
[CustomEditor(typeof(Part)), CanEditMultipleObjects]
public class PartEditor : Editor {

    SerializedProperty mass;
    SerializedProperty health;
    SerializedProperty damage;
    SerializedProperty models;
    SerializedProperty applicators;
    SerializedProperty connectedParts;
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
        mass = serializedObject.FindProperty("mass");
        health = serializedObject.FindProperty("health");
        damage = serializedObject.FindProperty("damage");
        models = serializedObject.FindProperty("models");
        applicators = serializedObject.FindProperty("applicators");
        connectedParts = serializedObject.FindProperty("connectedParts");
        DisplayModel();
    }

    public override void OnInspectorGUI() {
        if (Application.isPlaying && displayedGos != null) {
            ClearModel();
        }
        serializedObject.Update();
        EditorGUILayout.PropertyField(mass);
        EditorGUILayout.PropertyField(health);
        EditorGUILayout.PropertyField(damage);
        EditorList.Show(models, EditorListOption.ListLabel|EditorListOption.AddRemButtons);
        EditorList.Show(applicators, EditorListOption.ListLabel|EditorListOption.AddRemButtons);
        EditorList.Show(connectedParts, EditorListOption.ListLabel|EditorListOption.AddRemButtons);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

    protected virtual void OnDisable() {
        ClearModel();
    }
}
