using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "wheel", menuName = "Modules/wheel")]
public class WheelModule: Part {
    public ModelReference frame;
    public ModelReference steering;
    public SteeringJointApplicator steeringJoint;
    public ModelReference hub;
    public HubJointApplicator hubJoint;
    public ModelReference wheel;

    public override void Display(
        IDisplayer displayer
    ) {
        if (frame != null) {
            frame.Display(displayer);
        }
        if (steering != null) {
            steering.Display(displayer);
        }
        if (hub != null) {
            hub.Display(displayer);
        }
        if (wheel != null) {
            wheel.Display(displayer);
        }
        base.Display(displayer);
    }

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
        if (steering != null) {
            steeringBodyGo = PartUtil.BuildEmptyBody(partsGo, "steering.body");
            steering.Build(config, steeringBodyGo, "steering");
            // steering joint is attached to the rigidbody for steering, connect joint to top-level rigidbody
            if (steeringJoint != null) {
                steeringJoint.Apply(null, steeringBodyGo);
                var joiner = steeringBodyGo.GetComponent<Joiner>();
                if (joiner != null) {
                    joiner.Join(bodyGo.GetComponent<Rigidbody>());
                }
            }
        }

        // hub/wheel goes next under parts
        if (hub != null) {
            hubBodyGo = PartUtil.BuildEmptyBody(partsGo, "hub.body");
            hub.Build(config, hubBodyGo, "hub");
            if (wheel != null) {
                wheel.Build(config, hubBodyGo, "wheel");
            }
            // hub joint is attached to the rigidbody for the hub and connected to either the steering body (if present), or the top-level rigidbody
            if (hubJoint != null) {
                hubJoint.Apply(null, hubBodyGo);
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
