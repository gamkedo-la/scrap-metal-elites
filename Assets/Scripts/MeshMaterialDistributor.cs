using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaterialDistributor : MonoBehaviour {
    public Material material;

    private MeshMaterialApplicator[] applicators;
    private bool firstUpdate = true;

    void Start() {
        applicators = GetComponentsInChildren<MeshMaterialApplicator>();
        Debug.Log("# material applicators: " + applicators.Length);
    }


    void SetMaterials() {
        if (applicators == null) return;
        for (var i=0; i<applicators.Length; i++) {
            applicators[i].material = material;
        }
    }

    void Update() {
        if (firstUpdate) {
            SetMaterials();
            firstUpdate = false;
        }
    }

}
