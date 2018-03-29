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

    public virtual void Display(IDisplayer displayer) {
        if (models != null) {
            for (var i=0; i<models.Length; i++) {
                models[i].Display(displayer);
            }
        }
        if (connectedParts != null) {
            for (var i=0; i<connectedParts.Length; i++) {
                connectedParts[i].Display(displayer);
            }
        }
    }

    public virtual GameObject Build(
        GameObject root,
        string label
    ) {
        if (label == null || label == "") {
            label = name;
        }

        // create empty parts container
        var partsGo = new GameObject(label);
        // FIXME: make sure translation isn't happening here
        partsGo.transform.parent = root.transform;

        // create new rigid body for this part, set parts container as parent
        var rigidbodyGo = new GameObject(label + ".body", typeof(Rigidbody));
        rigidbodyGo.transform.parent = partsGo.transform;

        // apply applicators to parts container
        if (applicators != null) {
            for (var i=0; i<applicators.Length; i++) {
                applicators[0].Apply(partsGo);
            }
        }

        // instantiate model under rigid body
        if (models != null) {
            for (var i=0; i<models.Length; i++) {
                models[i].Build(rigidbodyGo, label + ".model");
            }
        }

        // instantiate connected parts
        if (connectedParts != null && connectedParts.Length > 0) {
            for (var i=0; i<connectedParts.Length; i++) {
                var childGo = connectedParts[i].Build(partsGo, label + ".part");
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
