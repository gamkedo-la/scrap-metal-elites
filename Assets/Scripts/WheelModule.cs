using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "wheel", menuName = "Modules/wheel")]
public class WheelModule: Part {
    public ModelReference frame;
    public PartReference steering;
    public SteeringJointApplicator steeringJoint;
    public PartReference hub;
    public HubJointApplicator hubJoint;
    public ModelReference wheel;

    public override GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject partsGo = null;
        GameObject bodyGo = null;
        GameObject steeringBodyGo = null;
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
        if (steering != null && steeringJoint != null) {
            var steeringGo = steering.Build(config, partsGo, "steering");
            if (steeringGo != null) {
                steeringBodyGo = PartUtil.GetBodyGo(steeringGo);
                // steering joint is attached to the rigidbody for steering, connect joint to top-level rigidbody
                if (steeringJoint != null) {
                    steeringJoint.Apply(config, steeringBodyGo);
                    var joiner = steeringBodyGo.GetComponent<Joiner>();
                    if (joiner != null) {
                        joiner.Join(bodyGo.GetComponent<Rigidbody>());
                    }
                }
            }
        }

        // hub/wheel goes next under parts
        if (hub != null && hubJoint != null) {
            var hubGo = hub.Build(config, partsGo, "hub");
            // if hub part isn't specified, build dummy hub rigidbody for wheel
            if (hubGo == null) {
                hubBodyGo = PartUtil.BuildGo(config, partsGo, "hub.body", typeof(Rigidbody));
            } else {
                hubBodyGo = PartUtil.GetBodyGo(hubGo);
            }
            if (wheel != null) {
                wheel.Build(config, hubBodyGo, "wheel");
            }
            // hub joint is attached to the rigidbody for the hub and connected to either the steering body (if present), or the top-level rigidbody
            if (hubJoint != null) {
                hubJoint.Apply(config, hubBodyGo);
                var joiner = hubBodyGo.GetComponent<Joiner>();
                if (joiner != null) {
                    var connectGo = (steeringBodyGo != null) ? steeringBodyGo : bodyGo;
                    joiner.Join(connectGo.GetComponent<Rigidbody>());
                }
            }
        }

        return partsGo;
    }
}
