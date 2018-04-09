using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDistributor : MonoBehaviour {
    public MaterialTag materialTag;

    private MaterialActuator[] actuators;
    private bool firstUpdate = true;

    void Start() {
        actuators = PartUtil.GetComponentsInChildren<MaterialActuator>(gameObject);
        Debug.Log("# material actuators: " + actuators.Length);
    }

    public void SetMaterials(MaterialTag tag) {
        if (actuators == null) return;
        for (var i=0; i<actuators.Length; i++) {
            actuators[i].Assign(tag);
        }
    }

    void Update() {
        if (firstUpdate) {
            SetMaterials(materialTag);
            firstUpdate = false;
        }
    }

}
