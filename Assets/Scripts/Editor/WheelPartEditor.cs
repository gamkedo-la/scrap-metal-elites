using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(WheelPart)), CanEditMultipleObjects]
public class WheelPartEditor : PartEditor {
    SerializedProperty mountPointProp;
    private static Vector3 pointSnap = Vector3.one * 0.1f;

    protected override void OnEnable() {
        // Setup the SerializedProperties.
        mountPointProp = serializedObject.FindProperty("mountPoint");
        base.OnEnable();
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(mountPointProp, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

    void OnSceneGUI(SceneView sceneView) {
        var part = target as WheelPart;
        Vector3 newPoint = Handles.FreeMoveHandle(part.mountPoint, Quaternion.identity, 0.05f, pointSnap, Handles.SphereHandleCap);
        if (newPoint != part.mountPoint) {
            Undo.RecordObject(part, "Move");
            part.mountPoint = newPoint;
		}
    }

    protected override void OnDisable() {
        base.OnDisable();
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }
}
