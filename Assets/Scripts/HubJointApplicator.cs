using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "hubJoint", menuName = "Joints/Hub")]
public class HubJointApplicator : ComponentApplicator {
    public bool applyBreakForce;
    public FloatReference breakForce;
    public bool applyBreakTorque;
    public FloatReference breakTorque;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add fixed joint component to target
        var joint = rigidbodyGo.AddComponent<HingeJoint>();

        // apply break limits, as specified
        if (applyBreakForce && breakForce != null) {
            joint.breakForce = breakForce.Value;
        }
        if (applyBreakTorque && breakTorque != null) {
            joint.breakTorque = breakTorque.Value;
        }

        // apply hinge properties
        joint.axis = new Vector3(1,0,0);

        // add simple joiner script to target, allowing quick joint join
        var joiner = target.AddComponent<Joiner>();
        joiner.joint = joint;
    }
}
