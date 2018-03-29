using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditorModelDisplayer : IDisplayer {
    Vector3 currentOffset;
    Quaternion currentRotation;
    List<GameObject> displayedModels;

    public EditorModelDisplayer(
        Vector3 offset,
        Vector3 rotation
    ) {
        this.currentOffset = offset;
        AddEulerRotation(rotation);
        displayedModels = new List<GameObject>();
    }
    public EditorModelDisplayer(
    ) {
        this.currentOffset = Vector3.zero;
        this.currentRotation = Quaternion.identity;
    }

    public void AddOffsetRotation(
        Vector3 offset,
        Vector3 rotation
    ) {
        currentOffset += offset;
        AddEulerRotation(rotation);
    }

    public void Display(
        Vector3 offset,
        Vector3 rotation,
        GameObject prefab
    ) {
        // display model prefab
        if (prefab != null) {
            var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            go.hideFlags = HideFlags.HideAndDontSave;
            go.transform.position = currentOffset + offset;
            AddEulerRotation(rotation);
            go.transform.rotation = currentRotation;
            AddEulerRotation(-rotation);
            displayedModels.Add(go);
        }
    }

    public void Clear() {
        if (displayedModels != null) {
            for (var i=0; i<displayedModels.Count; i++) {
                if (displayedModels[i] != null) {
                    Object.DestroyImmediate(displayedModels[i]);
                    displayedModels[i] = null;
                }
            }
        }
        displayedModels = new List<GameObject>();
    }

    void AddEulerRotation(
        Vector3 rotation
    ) {
        var qRot = Quaternion.identity;
        qRot.eulerAngles = rotation;
        currentRotation *= qRot;
    }
}
