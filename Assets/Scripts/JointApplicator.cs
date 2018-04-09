using UnityEngine;
using System.Collections;

public abstract class JointApplicator : ComponentApplicator {
    public bool applyBreakForce;
    public float breakForce;
    public bool applyBreakTorque;
    public float breakTorque;
    // allow joint to be damaged?
    public bool applyDamageToForce;
    public bool applyDamageToTorque;

    protected abstract Joint ApplyJoint(GameObject rigidbodyGo, PartConfig config, GameObject target);

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add fixed joint component to target
        var joint = ApplyJoint(rigidbodyGo, config, target);
        if (joint == null) return;
        //joint.enablePreprocessing = false;

        // apply break limits, as specified
        if (applyBreakForce) {
            joint.breakForce = breakForce;
        }
        if (applyBreakTorque) {
            joint.breakTorque = breakTorque;
        }

        // add damage actuator to joint rigidbody if joint can be damaged
        if (applyDamageToForce || applyDamageToTorque) {
            var actuator = rigidbodyGo.AddComponent<JointDamageActuator>();
            actuator.applyDamageToForce = applyDamageToForce;
            actuator.applyDamageToTorque = applyDamageToTorque;
        }

        // add simple joiner script to target, allowing quick joint join
        var joiner = target.AddComponent<Joiner>();
        joiner.joint = joint;
    }
}
