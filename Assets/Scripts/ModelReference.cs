using UnityEngine;

[System.Serializable]
public class ModelReference {
    public string label;
    public Vector3 offset;
    public Vector3 rotation;
    public GameObject model;

    public void Display(
        IDisplayer displayer
    ) {
        displayer.Display(offset, rotation, model);
    }

    public GameObject Build(
        GameObject root,
        string label
    ) {
        GameObject modelGo = null;
        if (model != null) {
            modelGo = Object.Instantiate(model, root.transform) as GameObject;
            // preserve model's original rotation (prior to parenting)
            modelGo.transform.localRotation = Quaternion.identity;
            modelGo.name = label + ".model";
            modelGo.transform.position = offset;
            modelGo.transform.eulerAngles = rotation;
        }
        return modelGo;
    }

}
