using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "turretJoint", menuName = "Joints/Turret")]
public class TurretJointApplicator : JointApplicator {
    [Tooltip("how fast will the turret spin")]
    public float turnSpeed = 50f;
    [Tooltip("how much force to apply to turn the turret")]
    public float turnForce = 10f;
    [Tooltip("which axis should the turret spin on")]
    public int axis = 1; // 0 => x, 1 => y, 2 => z
    [Tooltip("where should the hinge joint be anchored")]
    public Vector3 anchor = Vector3.zero;
    public bool debug;

    protected override Joint ApplyJoint(GameObject rigidbodyGo, PartConfig config, GameObject target) {

        // add joint component to target
        var joint = rigidbodyGo.AddComponent<HingeJoint>();
        // apply hinge properties
        if (axis == 2) {
            joint.axis = new Vector3(0,0,1);
        } else if (axis == 1) {
            joint.axis = new Vector3(0,1,0);
        } else {
            joint.axis = new Vector3(1,0,0);
        }
        joint.anchor = anchor;

        // setup joint motor
        var motor = joint.motor;
        motor.force = turnForce;
        joint.motor = motor;
        joint.useMotor = true;

        // add actuator
        var actuator = rigidbodyGo.AddComponent<TurretActuator>();
        actuator.turnSpeed = turnSpeed;
        actuator.turnForce = turnForce;
        actuator.debug = debug;
        return joint;

    }
}
