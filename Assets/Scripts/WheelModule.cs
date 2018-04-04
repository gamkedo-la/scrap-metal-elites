using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "wheel", menuName = "Modules/wheel")]
public class WheelModule: Part {
    [Tooltip("Mesh/Collider model/prefab to be associated with module frame, null means no model will be displayed for module frame")]
    public ModelReference frame;
    [Tooltip("Steering part to associate w/ wheel, null means no steering")]
    public PartReference steering;
    //public SteeringJointApplicator steeringJoint;
    [Tooltip("Hub part to associate w/ wheel, null means no hub/wheel")]
    public PartReference hub;
    //public HubJointApplicator hubJoint;
    [Tooltip("Wheel model to be added to hub body")] public ModelReference wheel;

    public override GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject partsGo = null;
        GameObject bodyGo = null;
        GameObject steeringGo = null;
        GameObject hubBodyGo = null;

        // build out part model first
        partsGo = base.Build(config, root, label);
        bodyGo = partsGo.transform.Find(partsGo.name + ".body").gameObject;

        // now build out wheel parts hierarchy
        // frame is instantiated under parts.body
        if (frame != null) {
            frame.Build(config, bodyGo, "frame");
        }

        // steering goes next (if specified) under parts
        if (steering != null) {
            steeringGo = steering.Build(config, partsGo, "steering");
        }

        // hub/wheel goes next under parts
        if (hub != null) {
            var hubGo = hub.Build(config, (steeringGo != null) ? steeringGo : partsGo, "hub");
            // if hub part isn't specified, build dummy hub rigidbody for wheel
            if (hubGo == null) {
                hubBodyGo = PartUtil.BuildGo(config, partsGo, "hub.body", typeof(Rigidbody));
            } else {
                hubBodyGo = PartUtil.GetBodyGo(hubGo);
            }
            if (wheel != null) {
                wheel.Build(config, hubBodyGo, "wheel");
            }
        }

        return partsGo;
    }
}
