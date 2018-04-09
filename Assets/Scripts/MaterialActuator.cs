using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialActuator : MonoBehaviour {
    public MeshMaterialMap[] materials;
    public bool debug;

    public void Assign(MaterialTag tag) {
        if (materials == null) return;

        // find material associated w/ tag
        Material material = null;
        for (var i=0; i<materials.Length; i++) {
            if (materials[i].tag == tag) {
                material = materials[i].material;
                break;
            }
        }
        if (material == null) return;

        // iterate through material changers in children, assigning new material
        var changers = GetComponentsInChildren<MaterialChanger>();
        for (var i=0; i<changers.Length; i++) {
            if (debug) Debug.Log(name + " changing material to " + tag.ToString());
            changers[i].material = material;
        }
    }
}
