using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class PartBuilder : MonoBehaviour {
    public Part part;

    void Clear() {
    }

    void Start() {
        if (part != null) {
            part.Build(null, gameObject, name);
        }
    }

}
