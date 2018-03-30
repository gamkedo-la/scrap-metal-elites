using UnityEngine;

[System.Serializable]
public class PartReference {
    public string label;
    public Vector3 offset;
    public Vector3 rotation;
    public Part part;

    public GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject partGo = null;
        if (part != null) {
            partGo = part.Build(config, root, (this.label == null) ? this.label : label);
            partGo.transform.position = offset;
            partGo.transform.eulerAngles = rotation;
        }
        return partGo;
    }

}
