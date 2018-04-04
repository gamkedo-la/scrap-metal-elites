using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(WheelModule)), CanEditMultipleObjects]
public class WheelModuleEditor : PartEditor {

    SerializedProperty mass;
    SerializedProperty health;
    SerializedProperty damage;
    SerializedProperty applicators;
    SerializedProperty frame;
    SerializedProperty steering;
    //SerializedProperty steeringJoint;
    SerializedProperty hub;
    //SerializedProperty hubJoint;
    SerializedProperty wheel;

    protected override void OnEnable() {
        // Setup the SerializedProperties.
        // from base
        mass = serializedObject.FindProperty("mass");
        health = serializedObject.FindProperty("health");
        damage = serializedObject.FindProperty("damage");
        applicators = serializedObject.FindProperty("applicators");
        // from wheel module
        frame = serializedObject.FindProperty("frame");
        steering = serializedObject.FindProperty("steering");
        //steeringJoint = serializedObject.FindProperty("steeringJoint");
        hub = serializedObject.FindProperty("hub");
        //hubJoint = serializedObject.FindProperty("hubJoint");
        wheel = serializedObject.FindProperty("wheel");
        base.OnEnable();
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUIUtility.labelWidth = 60f;
        EditorGUILayout.PropertyField(frame, true);
        EditorGUILayout.PropertyField(mass, true);
        EditorGUILayout.PropertyField(health, true);
        EditorGUILayout.PropertyField(damage, true);
        EditorList.Show(applicators, EditorListOption.ListLabel|EditorListOption.AddRemButtons);
        EditorGUILayout.PropertyField(steering, true);
        //EditorGUILayout.PropertyField(steeringJoint);
        EditorGUILayout.PropertyField(hub, true);
        //EditorGUILayout.PropertyField(hubJoint);
        EditorGUILayout.PropertyField(wheel, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

}
