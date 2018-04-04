using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(Part))]
[CustomEditor(typeof(TestList)), CanEditMultipleObjects]
public class TestListEditor : Editor {

    SerializedProperty ints;
    SerializedProperty vecs;
    SerializedProperty parts;
    SerializedProperty modules;

    protected virtual void OnEnable() {
        // Setup the SerializedProperties.
        ints = serializedObject.FindProperty("ints");
        vecs = serializedObject.FindProperty("vecs");
        parts = serializedObject.FindProperty("parts");
        modules = serializedObject.FindProperty("modules");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorList.Show(ints, EditorListOption.AddRemButtons|EditorListOption.MoveButton);
        EditorList.Show(vecs, EditorListOption.AddRemButtons);
        EditorList.Show(parts, EditorListOption.AddRemButtons);
        EditorList.Show(modules, EditorListOption.ElementLabels|EditorListOption.ListLabel|EditorListOption.AddRemButtons);

        //EditorGUILayout.PropertyField(ints, true);
        //EditorGUILayout.PropertyField(vecs, true);
        //EditorGUILayout.PropertyField(parts, true);
        serializedObject.ApplyModifiedProperties();
    }
}
