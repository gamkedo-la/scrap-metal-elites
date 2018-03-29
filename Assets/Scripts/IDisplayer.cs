using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public interface IDisplayer {
    void AddOffsetRotation(
        Vector3 offset,
        Vector3 rotation
    );

    void Display(
        Vector3 offset,
        Vector3 rotation,
        GameObject prefab
    );

    void Clear();
}
