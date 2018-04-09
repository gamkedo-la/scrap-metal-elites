using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshMaterialMap {
    public MaterialTag tag;
    public Material material;
}

[CreateAssetMenu(fileName = "materialMap", menuName = "Variable/Material")]
public class MaterialApplicator : ComponentApplicator {
    public MeshMaterialMap[] materials;
    public bool debug;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add health component
        var actuator = rigidbodyGo.AddComponent<MaterialActuator>();
        actuator.materials = materials;
        actuator.debug = debug;
    }
}
