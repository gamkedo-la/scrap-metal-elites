using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpinnerWeaponModule)), CanEditMultipleObjects]
public class SpinnerWeaponModuleEditor : PartEditor {

    SerializedProperty frameProp;
    SerializedProperty spinnerJointProp;
    SerializedProperty spinnerProp;

    protected override void OnEnable() {
        // Setup the SerializedProperties.
        frameProp = serializedObject.FindProperty("frame");
        spinnerJointProp = serializedObject.FindProperty("spinnerJoint");
        spinnerProp = serializedObject.FindProperty("spinner");
        base.OnEnable();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(frameProp, true);
        EditorGUILayout.PropertyField(spinnerJointProp);
        EditorGUILayout.PropertyField(spinnerProp, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

}
