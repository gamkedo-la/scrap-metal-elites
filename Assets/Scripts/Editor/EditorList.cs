using System;
using UnityEditor;
using UnityEngine;

[Flags]
public enum EditorListOption {
	None = 0,
	ListSize = 1,
	ListLabel = 2,
    ElementLabels = 4,
    AddRemButtons = 8,
    MoveButton = 16,
	Default = ListSize | ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Default | AddRemButtons | MoveButton
}

public static class EditorList {
    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
    private static GUIContent moveButtonContent = new GUIContent("\u21b4", "move down");
	private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
	private static GUIContent deleteButtonContent = new GUIContent("-", "delete");
	private static GUIContent addButtonContent = new GUIContent("+", "add element");

	public static void Show (SerializedProperty list, EditorListOption options = EditorListOption.Default) {
        if (!list.isArray) {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }
        bool showListLabel = (options & EditorListOption.ListLabel) != 0;
        bool showListSize = (options & EditorListOption.ListSize) != 0;
        if (showListLabel) {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }

        if (!showListLabel || list.isExpanded) {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize) {
                EditorGUILayout.PropertyField(size);
            }
            if (size.hasMultipleDifferentValues) {
                EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
            } else {
                ShowElements(list, options);
            }
        }
        if (showListLabel) {
            EditorGUI.indentLevel -= 1;
        }
	}

    private static void ShowElements (SerializedProperty list, EditorListOption options) {
		bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;
		bool showAddRemButtons = (options & EditorListOption.AddRemButtons) != 0;
		bool showMoveButton = (options & EditorListOption.MoveButton) != 0;
		for (int i = 0; i < list.arraySize; i++) {
            if (showAddRemButtons || showMoveButton) {
                EditorGUILayout.BeginHorizontal();
            }
			if (showElementLabels) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), true);
			}
			else {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none, true);
			}
            if (showAddRemButtons || showMoveButton) {
                ShowButtons(showAddRemButtons, showMoveButton, list, i);
                EditorGUILayout.EndHorizontal();
            }
		}
        if (showAddRemButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
			list.arraySize += 1;
		}
	}

    private static void ShowButtons (bool showAddRemButtons, bool showMoveButton, SerializedProperty list, int index) {
        if (showAddRemButtons) {
            if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth)) {
                list.InsertArrayElementAtIndex(index);
            }
            if (GUILayout.Button(deleteButtonContent, (showMoveButton) ? EditorStyles.miniButtonMid : EditorStyles.miniButtonRight, miniButtonWidth)) {
                int oldSize = list.arraySize;
                list.DeleteArrayElementAtIndex(index);
                if (list.arraySize == oldSize) {
                    list.DeleteArrayElementAtIndex(index);
                }
            }
        }
        if (showMoveButton) {
            if (GUILayout.Button(moveButtonContent, (showAddRemButtons) ? EditorStyles.miniButtonRight : EditorStyles.miniButton, miniButtonWidth)) {
                list.MoveArrayElement(index, index + 1);
            }
        }
    }

}
