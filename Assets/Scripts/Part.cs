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
    public GameObject modelPrefab;
    public ComponentApplicator[] applicators;
    public PartImport[] connectedParts;

    public GameObject Build(
        GameObject root,
        string label
    ) {
        if (label == null || label == "") {
            label = name;
        }

        // create empty parts container
        var partsGo = new GameObject(label);
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
        if (modelPrefab != null) {
            var modelGo = Instantiate(modelPrefab, rigidbodyGo.transform) as GameObject;
            // preserve model's original rotation (prior to parenting to rigidbodyGo)
            modelGo.transform.localRotation = Quaternion.identity;
            modelGo.name = label + ".model";
        }

        // instantiate connected parts
        if (connectedParts != null && connectedParts.Length > 0) {
            for (var i=0; i<connectedParts.Length; i++) {
                if (connectedParts[i].part != null) {
                    var childGo = connectedParts[i].part.Build(partsGo, connectedParts[i].label);
                    if (childGo != null) {
                        // set position/rotation of child
                        childGo.transform.position = connectedParts[i].modelOffset;
                        childGo.transform.eulerAngles = connectedParts[i].modelRotation;

                        // join child part to current rigidbody
                        var joiner = childGo.GetComponent<Joiner>();
                        if (joiner != null) {
                            joiner.Join(rigidbodyGo.GetComponent<Rigidbody>());
                        }
                    }
                }
            }
        }
        return partsGo;
    }
}
