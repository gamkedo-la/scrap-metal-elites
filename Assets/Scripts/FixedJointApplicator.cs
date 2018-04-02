using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "fixedJoint", menuName = "Joints/Fixed")]
public class FixedJointApplicator : JointApplicator {
    protected override Joint ApplyJoint(GameObject rigidbodyGo, PartConfig config, GameObject target) {
        return rigidbodyGo.AddComponent<FixedJoint>();
    }
}
