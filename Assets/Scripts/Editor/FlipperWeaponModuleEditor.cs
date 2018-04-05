using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(FlipperWeaponModule)), CanEditMultipleObjects]
public class FlipperWeaponModuleEditor : PartEditor {

    SerializedProperty mass;
    SerializedProperty health;
    SerializedProperty damage;
    SerializedProperty applicators;

    SerializedProperty frame;
    SerializedProperty flipper;

    protected override void OnEnable() {
        // Setup the SerializedProperties.
        mass = serializedObject.FindProperty("mass");
        health = serializedObject.FindProperty("health");
        damage = serializedObject.FindProperty("damage");
        applicators = serializedObject.FindProperty("applicators");
        frame = serializedObject.FindProperty("frame");
        flipper = serializedObject.FindProperty("flipper");
        base.OnEnable();
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUIUtility.labelWidth = 60f;
        EditorGUILayout.PropertyField(frame, true);
        EditorGUILayout.PropertyField(mass, true);
        EditorGUILayout.PropertyField(health, true);
        EditorGUILayout.PropertyField(damage, true);
        EditorList.Show(applicators, EditorListOption.ListLabel|EditorListOption.AddRemButtons);
        EditorGUILayout.PropertyField(flipper, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

}
