using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PartConfigRow))]
public class PartConfigRowDrawer : PropertyDrawer {
    /// <summary>
    /// Options to display in the popup to select constant or variable.
    /// </summary>
    // pop up options need to align w/ value of valueType integer in record
    private readonly string[] popupOptions =
        { "Bool", "String", "Float" };

    /// <summary> Cached style to use to draw the popup button. </summary>
    private GUIStyle popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        var contentPosition = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties
        SerializedProperty key = property.FindPropertyRelative("key");
        SerializedProperty valueType = property.FindPropertyRelative("valueType");
        SerializedProperty boolValue = property.FindPropertyRelative("boolValue");
        SerializedProperty stringValue = property.FindPropertyRelative("stringValue");
        SerializedProperty floatValue = property.FindPropertyRelative("floatValue");

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // split row into two
        Vector2 cellSize = new Vector2(contentPosition.size.x/2f, contentPosition.size.y);
        Rect leftCell = new Rect(contentPosition.min, cellSize);
        Rect rightCell = new Rect(contentPosition.min + new Vector2(cellSize.x,0), cellSize);

        // left cell gets the key property
		EditorGUIUtility.labelWidth = 14f;
        EditorGUI.PropertyField(leftCell, key, new GUIContent("K"));

        // right cell gets the type popup and value property
        // Calculate rect for configuration button
        Rect buttonRect = new Rect(rightCell);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        rightCell.xMin = buttonRect.xMax;
        int result = EditorGUI.Popup(buttonRect, valueType.intValue, popupOptions, popupStyle);
        valueType.intValue = result;

        if (result == 0) {
            EditorGUI.PropertyField(rightCell, boolValue, new GUIContent("V"));
        } else if (result == 1) {
            EditorGUI.PropertyField(rightCell, stringValue, new GUIContent("V"));
        } else if (result == 2) {
            EditorGUI.PropertyField(rightCell, floatValue, new GUIContent("V"));
        }

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
