using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "spinner", menuName = "Modules/Spinner Weapon")]
public class SpinnerWeaponModule: Part {
    [Tooltip("Mesh/Collider model/prefab to be associated with module frame, null means no model will be displayed for module frame")]
    public ModelReference frame;
    [Tooltip("Rotating Spinner part")]
    public PartReference spinner;

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

        // now build out parts hierarchy - frame is instantiated under parts.body
        if (frame != null) {
            frame.Build(config, bodyGo, "frame");
        }

        // spinner goes next (if specified) under parts
        if (spinner != null) {
            spinner.Build(config, partsGo, "spinner");
            /*
            if (spinnerGo != null) {
                var spinnerBodyGo = PartUtil.GetBodyGo(spinnerGo);
                if (spinnerBodyGo != null) {
                    spinnerJoint.Apply(config, spinnerBodyGo);
                    var joiner = spinnerBodyGo.GetComponent<Joiner>();
                    if (joiner != null) {
                        joiner.Join(bodyGo.GetComponent<Rigidbody>());
                    }
                }
            }
            */
        }

        return partsGo;
    }
}
