using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "FixedJoint", menuName = "Parts/FixedJoint")]
public class FixedJointApplicator : ComponentApplicator {
    public bool applyBreakForce;
    public float breakForce;
    public bool applyBreakTorque;
    public float breakTorque;

    public override void Apply(GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = target.transform.Find(target.name + ".body").gameObject;
        if (rigidbodyGo == null) {
            if (target.GetComponent<Rigidbody>() != null) {
                rigidbodyGo = target;
            }
        }

        // add fixed joint component to target
        var joint = rigidbodyGo.AddComponent<FixedJoint>();
        // apply break limits, as specified
        if (applyBreakForce) {
            joint.breakForce = breakForce;
        }
        if (applyBreakTorque) {
            joint.breakTorque = breakTorque;
        }

        // add simple joiner script to target, allowing quick joint join
        var joiner = target.AddComponent<Joiner>();
        joiner.joint = joint;
    }
}
