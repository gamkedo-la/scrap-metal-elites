using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ModelReference {
    public string label;
    public Vector3 offset;
    public Vector3 rotation;
    public GameObject model;

    public GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject modelGo = null;
        if (model != null) {
            if (Application.isPlaying) {
                modelGo = Object.Instantiate(model, (root != null) ? root.transform : null) as GameObject;
            } else {
                modelGo = PrefabUtility.InstantiatePrefab(model) as GameObject;
                modelGo.hideFlags = HideFlags.HideAndDontSave;
                if (root != null) {
                    modelGo.transform.parent = root.transform;
                }
            }
            // preserve model's original rotation (prior to parenting)
            modelGo.transform.localRotation = Quaternion.identity;
            modelGo.name = label + ".model";
            modelGo.transform.localPosition = offset;
            modelGo.transform.localEulerAngles = rotation;
        }
        return modelGo;
    }

}
