using UnityEngine;
using System.Collections;

[System.Serializable]
public class PartImport {
    public string label;
    public Vector3 modelOffset;
    public Vector3 modelRotation;
    public Part part;
}

[CreateAssetMenu(fileName = "Part", menuName = "Parts/Part")]
public class Part : ScriptableObject {
    public ModelReference[] models;
    public ComponentApplicator[] applicators;
    public PartReference[] connectedParts;

    public virtual GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        if (label == null || label == "") {
            label = name;
        }

        // create empty parts container
        var partsGo = PartUtil.BuildGo(config, root, label);

        // create new rigid body for this part, set parts container as parent
        var rigidbodyGo = PartUtil.BuildGo(config, partsGo, label + ".body", typeof(Rigidbody));

        // apply applicators to parts container
        if (applicators != null) {
            for (var i=0; i<applicators.Length; i++) {
                applicators[0].Apply(null, partsGo);
            }
        }

        // instantiate model under rigid body
        if (models != null) {
            for (var i=0; i<models.Length; i++) {
                models[i].Build(config, rigidbodyGo, label + ".model");
            }
        }

        // instantiate connected parts
        if (connectedParts != null && connectedParts.Length > 0) {
            for (var i=0; i<connectedParts.Length; i++) {
                var childGo = connectedParts[i].Build(config, partsGo, label + ".part");
                if (childGo != null) {
                    // join child part to current rigidbody
                    var joiner = childGo.GetComponent<Joiner>();
                    if (joiner != null) {
                        joiner.Join(rigidbodyGo.GetComponent<Rigidbody>());
                    }
                }
            }
        }
        return partsGo;
    }
}
