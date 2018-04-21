using UnityEngine;
using System.Collections;

public abstract class JointApplicator : ComponentApplicator {
    [Header("Joint Damage Config")]
    [Tooltip("allow the joint to break based on impact force")]
    public bool applyBreakForce;
    public float breakForce;
    [Tooltip("allow the joint to break based on torque force")]
    public bool applyBreakTorque;
    public float breakTorque;
    [Tooltip("allow damage to joint, weakening joint")]
    public bool applyDamageToForce;
    public bool applyDamageToTorque;

    [Header("Event Channels")]
    public GameRecordEvent gameEventChannel;

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
            actuator.gameEventChannel = gameEventChannel;
        }

        // add simple joiner script to target, allowing quick joint join
        var joiner = target.AddComponent<Joiner>();
        joiner.joint = joint;
    }
}
