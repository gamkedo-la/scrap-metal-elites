using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDistributor : MonoBehaviour {
    public MaterialTag materialTag;
    private bool firstUpdate = true;


    public void SetMaterials(MaterialTag tag) {
        materialTag = tag;
        var actuators = PartUtil.GetComponentsInChildren<MaterialActuator>(gameObject);
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
