using UnityEngine;
using System.Collections;

public class PartReference : MonoBehaviour {
    public Part part;

    void Build() {
        if (part != null) {
            var gameObject = Instantiate(part.modelPrefab) as GameObject;
            gameObject.name = "partreferencebuild";
        }
    }

    void Clear() {
    }

    void Start() {
        if (part != null) {
            part.Build(gameObject, null);
        }
    }

}
