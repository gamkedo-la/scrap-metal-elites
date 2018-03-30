using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(BotModule)), CanEditMultipleObjects]
public class BotModuleEditor : PartEditor {

    SerializedProperty frameProp;
    SerializedProperty modulesProp;

    protected override void OnEnable() {
        // Setup the SerializedProperties.
        frameProp = serializedObject.FindProperty("frame");
        modulesProp = serializedObject.FindProperty("modules");
        base.OnEnable();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(frameProp, true);
        EditorGUILayout.PropertyField(modulesProp, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

}
