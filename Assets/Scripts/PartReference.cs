using UnityEngine;

[System.Serializable]
public class PartReference {
    public string label;
    public Vector3 offset;
    public Vector3 rotation;
    public Part part;

    public void Display(
        IDisplayer displayer
    ) {
        if (part != null) {
            displayer.AddOffsetRotation(offset, rotation);
            part.Display(displayer);
            displayer.AddOffsetRotation(-offset, -rotation);
        }
    }

    public GameObject Build(
        GameObject root,
        string label
    ) {
        GameObject partGo = null;
        if (part != null) {
            partGo = part.Build(root, (this.label == null) ? this.label : label);
            partGo.transform.position = offset;
            partGo.transform.eulerAngles = rotation;
        }
        return partGo;
    }

}
