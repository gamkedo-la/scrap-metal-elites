using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "flipperJoint", menuName = "Joints/Flipper")]
public class FlipperJointApplicator : JointApplicator {
    public bool motor = true;
    public float motorMaxTorque;
    public float motorMaxSpeed;
	public float impactForce = 400.0f;
    public int axis = 0; // 0 => x, 1 => y, 2 => z
    public Vector3 anchor = Vector3.zero;
    public float minAngle = 0f;
    public float maxAngle = 180f;
    public bool useSpring = false;
    public float springForce = 50f;
    public float springDamper = 5f;
    public bool reverse;

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
        if (useSpring) {
            var hingeSpring = joint.spring;
            hingeSpring.spring = springForce;
            hingeSpring.damper = springDamper;
            hingeSpring.targetPosition = (reverse) ? maxAngle : minAngle;
            joint.spring = hingeSpring;
            joint.useSpring = true;
        }
        var limits = joint.limits;
        limits.min = minAngle;
        limits.max = maxAngle;
        joint.limits = limits;
        joint.useLimits = true;
        joint.anchor = anchor;

        // add motor
        var applyMotor = motor && (motorMaxTorque > 0) && (motorMaxSpeed > 0);
        if (applyMotor) {
            var actuator = rigidbodyGo.AddComponent<FlipperActuator>();
            actuator.maxTorque = motorMaxTorque;
            actuator.maxSpeed = motorMaxSpeed;
            actuator.minAngle = minAngle;
            actuator.maxAngle = maxAngle;
            actuator.reverse = reverse;
            actuator.impactForce = impactForce;
            actuator.axis = axis;
            actuator.useSpring = useSpring;
        }
        return joint;

    }
}
