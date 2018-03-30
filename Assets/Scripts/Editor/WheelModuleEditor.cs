using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(WheelModule)), CanEditMultipleObjects]
public class WheelModuleEditor : PartEditor {

    SerializedProperty frameProp;
    SerializedProperty steeringProp;
    SerializedProperty steeringJointProp;
    SerializedProperty hubProp;
    SerializedProperty hubJointProp;
    SerializedProperty wheelProp;

    protected override void OnEnable() {
        // Setup the SerializedProperties.
        frameProp = serializedObject.FindProperty("frame");
        steeringProp = serializedObject.FindProperty("steering");
        steeringJointProp = serializedObject.FindProperty("steeringJoint");
        hubProp = serializedObject.FindProperty("hub");
        hubJointProp = serializedObject.FindProperty("hubJoint");
        wheelProp = serializedObject.FindProperty("wheel");
        base.OnEnable();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(frameProp, true);
        EditorGUILayout.PropertyField(steeringProp, true);
        EditorGUILayout.PropertyField(steeringJointProp);
        EditorGUILayout.PropertyField(hubProp, true);
        EditorGUILayout.PropertyField(hubJointProp);
        EditorGUILayout.PropertyField(wheelProp, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

}
