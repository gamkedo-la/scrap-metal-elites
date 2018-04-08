using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PanelApplicator)), CanEditMultipleObjects]
public class PanelApplicatorEditor : Editor {

    SerializedProperty top;
    SerializedProperty bottom;
    SerializedProperty left;
    SerializedProperty right;
    SerializedProperty front;
    SerializedProperty back;
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
        displayConfig.Save<bool>(ConfigTag.PartHide, true);
        displayConfig.Save<bool>(ConfigTag.PartDontSave, true);

        // build the part
        for (var i=0; i<targets.Length; i++) {
            var panel = (PanelApplicator) targets[i];
            if (panel.top != null) {
                displayedGos.Add(panel.top.Build(displayConfig, null, "display"));
            }
            if (panel.bottom != null) {
                displayedGos.Add(panel.bottom.Build(displayConfig, null, "display"));
            }
            if (panel.left != null) {
                displayedGos.Add(panel.left.Build(displayConfig, null, "display"));
            }
            if (panel.right != null) {
                displayedGos.Add(panel.right.Build(displayConfig, null, "display"));
            }
            if (panel.front != null) {
                displayedGos.Add(panel.front.Build(displayConfig, null, "display"));
            }
            if (panel.back != null) {
                displayedGos.Add(panel.back.Build(displayConfig, null, "display"));
            }
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
        top = serializedObject.FindProperty("top");
        bottom = serializedObject.FindProperty("bottom");
        front = serializedObject.FindProperty("front");
        back = serializedObject.FindProperty("back");
        left = serializedObject.FindProperty("left");
        right = serializedObject.FindProperty("right");
        DisplayModel();
    }

    public override void OnInspectorGUI() {
        if (Application.isPlaying && displayedGos != null) {
            ClearModel();
        }
        serializedObject.Update();
        EditorGUILayout.PropertyField(top);
        EditorGUILayout.PropertyField(bottom);
        EditorGUILayout.PropertyField(left);
        EditorGUILayout.PropertyField(right);
        EditorGUILayout.PropertyField(front);
        EditorGUILayout.PropertyField(back);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

    protected virtual void OnDisable() {
        ClearModel();
    }
}
