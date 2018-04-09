using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(BotModule)), CanEditMultipleObjects]
public class BotModuleEditor : PartEditor {

    SerializedProperty mass;
    SerializedProperty health;
    SerializedProperty damage;
    SerializedProperty applicators;

    SerializedProperty frame;
    SerializedProperty modules;

    protected override void OnEnable() {
        // from base
        mass = serializedObject.FindProperty("mass");
        health = serializedObject.FindProperty("health");
        damage = serializedObject.FindProperty("damage");
        applicators = serializedObject.FindProperty("applicators");
        // frame bot
        frame = serializedObject.FindProperty("frame");
        modules = serializedObject.FindProperty("modules");
        base.OnEnable();
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        serializedObject.Update();
        Undo.RecordObject(target, "Modify Bot Properties");
        EditorGUILayout.PropertyField(frame, true);
        EditorGUILayout.PropertyField(mass, true);
        EditorGUILayout.PropertyField(health, true);
        EditorGUILayout.PropertyField(damage, true);
        EditorList.Show(applicators, EditorListOption.ListLabel|EditorListOption.AddRemButtons);
        //EditorGUILayout.PropertyField(frame, true);
        EditorList.Show(modules, EditorListOption.ElementLabels|EditorListOption.ListLabel|EditorListOption.AddRemButtons);
        //EditorGUILayout.PropertyField(modules, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

}
