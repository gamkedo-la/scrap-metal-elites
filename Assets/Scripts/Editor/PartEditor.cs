using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PartDisplayer {
    Part part;
    Vector3 modelOffset;
    Vector3 modelRotation;
    GameObject displayedModel;
    PartDisplayer[] displayedParts;

    public PartDisplayer(
        Part part
    ) {
        this.modelOffset = Vector3.zero;
        this.modelRotation = Vector3.zero;
        this.part = part;
    }
    public PartDisplayer(
        Vector3 modelOffset,
        Vector3 modelRotation,
        Part part
    ) {
        this.modelOffset = modelOffset;
        this.modelRotation = modelRotation;
        this.part = part;
    }

    public void Display(
    ) {
        // clear any previous display
        Hide();

        if (part == null) return;

        // display model prefab
        if (part.modelPrefab != null) {
            displayedModel = PrefabUtility.InstantiatePrefab(part.modelPrefab) as GameObject;
            displayedModel.hideFlags = HideFlags.HideInHierarchy;
            displayedModel.transform.position = modelOffset;
            displayedModel.transform.eulerAngles = modelRotation;
        }

        // display connected parts
        displayedParts = new PartDisplayer[part.connectedParts.Length];
        for (var i=0; i<part.connectedParts.Length; i++) {
            if (part.connectedParts[i] != null) {
                displayedParts[i] = new PartDisplayer(
                    part.connectedParts[i].modelOffset,
                    part.connectedParts[i].modelRotation,
                    part.connectedParts[i].part
                );
                displayedParts[i].Display();
            }
        }
    }

    public void Hide() {
        if (displayedModel != null) {
            Object.DestroyImmediate(displayedModel);
        }
        if (displayedParts != null) {
            for (var i=0; i<displayedParts.Length; i++) {
                if (displayedParts[i] != null) {
                    displayedParts[i].Hide();
                }
            }
        }
    }
}

//[CustomEditor(typeof(Part))]
[CustomEditor(typeof(Part)), CanEditMultipleObjects]
public class PartEditor : Editor {

    SerializedProperty modelOffsetProp;
    SerializedProperty modelRotationProp;
    SerializedProperty modelPrefabProp;
    SerializedProperty connectedPartsProp;
    PartDisplayer[] displayedParts;

    void OnEnable() {
        Debug.Log("PartEditor OnEnable");
        // Setup the SerializedProperties.
        modelOffsetProp = serializedObject.FindProperty("modelOffset");
        modelRotationProp = serializedObject.FindProperty("modelRotation");
        modelPrefabProp = serializedObject.FindProperty("modelPrefab");
        connectedPartsProp = serializedObject.FindProperty("connectedParts");

        displayedParts = new PartDisplayer[targets.Length];
        for (var i=0; i<targets.Length; i++) {
            Debug.Log("on enable part: " + targets[i]);
            displayedParts[i] = new PartDisplayer((Part) targets[i]);
            displayedParts[i].Display();
        }

    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(modelOffsetProp);
        EditorGUILayout.PropertyField(modelRotationProp);
        EditorGUILayout.PropertyField(modelPrefabProp);
        EditorGUILayout.PropertyField(connectedPartsProp, true);
        serializedObject.ApplyModifiedProperties();
    }

    void OnDisable() {
        Debug.Log("PartEditor OnDisable");
        if (displayedParts != null) {
            for (var i=0; i<displayedParts.Length; i++) {
                if (displayedParts[i] != null) {
                    displayedParts[i].Hide();
                }
            }
            displayedParts = null;
        }
    }
}
