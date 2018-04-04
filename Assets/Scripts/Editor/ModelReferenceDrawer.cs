using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(ModelReference))]
public class ModelReferenceDrawer : PropertyDrawer {
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
        // 16 is default height for a row ... so three rows w/ buffer of 2 pixels inbetween
		return 16f + 18f + 18f;
	}
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);
        // draw property label
        var contentPosition = EditorGUI.PrefixLabel(position, label);
        // don't indent child properties
        var indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // setup child property rects
        var offsetRect = new Rect(contentPosition.x, position.y, contentPosition.width, 16f);
        var rotationRect = new Rect(contentPosition.x, position.y+18f, contentPosition.width, 16f);
        var modelRect = new Rect(contentPosition.x, position.y+36f, contentPosition.width, 16f);

        EditorGUIUtility.labelWidth = 60f;
        EditorGUI.PropertyField(offsetRect, property.FindPropertyRelative("offset"));
        EditorGUI.PropertyField(rotationRect, property.FindPropertyRelative("rotation"));
        EditorGUI.PropertyField(modelRect, property.FindPropertyRelative("model"), GUIContent.none);
        // restore indent level
        EditorGUI.indentLevel = indentLevel;
        EditorGUI.EndProperty();
    }
}
