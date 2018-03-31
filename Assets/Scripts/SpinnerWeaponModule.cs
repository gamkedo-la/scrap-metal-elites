using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "spinner", menuName = "Modules/Spinner Weapon")]
public class SpinnerWeaponModule: Part {
    public ModelReference frame;
    public SpinnerJointApplicator spinnerJoint;
    public ModelReference spinner;

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
        if (spinnerJoint != null && spinner != null) {
            var spinnerBodyGo = PartUtil.BuildGo(config, partsGo, "spinner.body", typeof(Rigidbody));
            spinner.Build(config, spinnerBodyGo, "spinner");
            // steering joint is attached to the rigidbody for steering, connect joint to top-level rigidbody
            spinnerJoint.Apply(config, spinnerBodyGo);
            var joiner = spinnerBodyGo.GetComponent<Joiner>();
            if (joiner != null) {
                joiner.Join(bodyGo.GetComponent<Rigidbody>());
            }
        }

        return partsGo;
    }
}
