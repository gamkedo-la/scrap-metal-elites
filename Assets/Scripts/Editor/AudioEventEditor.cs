using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AudioEvent), true)]
public class AudioEventEditor : Editor
{

	[SerializeField] private AudioEmitter _previewer;

	public void OnEnable()
	{
		_previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioEmitter)).GetComponent<AudioEmitter>();
	}

	public void OnDisable()
	{
		DestroyImmediate(_previewer.gameObject);
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
		if (GUILayout.Button("Play"))
		{
			((AudioEvent) target).Play(_previewer);
		}
		if (GUILayout.Button("Stop"))
		{
			((AudioEvent) target).Stop(_previewer);
		}
		EditorGUI.EndDisabledGroup();
	}
}
