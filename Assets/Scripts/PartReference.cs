using UnityEngine;

[System.Serializable]
public class PartReference {
    public Vector3 offset;
    public Vector3 rotation;
    [Tooltip("How is the referenced part to be attached to the parent object, null means no joint/connection")]
    public JointApplicator joint;
    public Part part;

    public GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject partGo = null;

        // instantiate part
        if (part != null) {
            partGo = part.Build(config, root, label != null ? label : part.name);
            partGo.transform.localPosition = offset;
            partGo.transform.localEulerAngles = rotation;
        }

        // joint specifies how part is to be attached to root, if specified, apply joint and join to root
        if (joint != null && partGo != null) {
            var partBodyGo = PartUtil.GetBodyGo(partGo);
            if (partBodyGo != null) {
                joint.Apply(config, partBodyGo);
                var joiner = partBodyGo.GetComponent<Joiner>();
                var rootBodyGo = PartUtil.GetBodyGo(root);
                if (joiner != null && rootBodyGo != null) {
                    joiner.Join(rootBodyGo.GetComponent<Rigidbody>());
                }
            }
        }

        return partGo;
    }

}
