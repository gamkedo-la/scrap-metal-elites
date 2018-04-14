using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "flipper", menuName = "Modules/Flipper Weapon")]
public class FlipperWeaponModule: Part {
    [Tooltip("mesh/collider model/prefab to be associated with module frame, null means no model will be displayed for module frame")]
    public ModelReference frame;
    [Tooltip("rotating flipper part, assign flipper joint for joint value")]
    public PartReference flipper;

    public override GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject partsGo = null;
        GameObject bodyGo = null;

        // build out part model first
        partsGo = base.Build(config, root, label);
        bodyGo = partsGo.transform.Find(partsGo.name + ".body").gameObject;
        bodyGo.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;

        // now build out parts hierarchy - frame is instantiated under parts.body
        if (frame != null) {
            frame.Build(config, bodyGo, "frame");
        }

        // flipper goes next (if specified) under parts
        if (flipper != null) {
            flipper.Build(config, partsGo, "flipper");
        }

        return partsGo;
    }
}
