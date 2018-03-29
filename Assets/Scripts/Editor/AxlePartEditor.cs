using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AxlePart)), CanEditMultipleObjects]
public class AxlePartEditor : PartEditor {

    SerializedProperty frameProp;
    SerializedProperty hubProp;
    SerializedProperty kingpinProp;
    //EditorModelDisplayer displayer;

    /*
    void Awake() {
        displayer = new EditorModelDisplayer();
    }
    */

    protected override void DisplayModel() {
        base.DisplayModel();
        if (displayer == null) return;
        // do not display prefab models if application is playing
        if (Application.isPlaying) return;

        // iterate through referenced objects
        for (var i=0; i<targets.Length; i++) {
            var part = (AxlePart) targets[i];
            displayer.Display(part.frame.offset, part.frame.rotation, part.frame.model);
            displayer.Display(part.hub.offset, part.hub.rotation, part.hub.model);
            displayer.Display(part.kingpin.offset, part.kingpin.rotation, part.kingpin.model);
        }
    }

    protected override void OnEnable() {
        // Setup the SerializedProperties.
        frameProp = serializedObject.FindProperty("frame");
        hubProp = serializedObject.FindProperty("hub");
        kingpinProp = serializedObject.FindProperty("kingpin");
        //DisplayModel();
        base.OnEnable();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(frameProp, true);
        EditorGUILayout.PropertyField(hubProp, true);
        EditorGUILayout.PropertyField(kingpinProp, true);
        if (serializedObject.ApplyModifiedProperties()) {
            DisplayModel();
        }
    }

}
