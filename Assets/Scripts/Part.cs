using UnityEngine;
using System.Collections;

[System.Serializable]
public class PartImport {
    public Vector3 modelOffset;
    public Vector3 modelRotation;
    public Part part;
}

[CreateAssetMenu(fileName = "Part", menuName = "Part", order = 1)]
public class Part : ScriptableObject {
    public Vector3 modelOffset;
    public Vector3 modelRotation;
    public GameObject modelPrefab;
    public PartImport[] connectedParts;
}
