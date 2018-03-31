using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "spinnerJoint", menuName = "Joints/Spinner")]
public class SpinnerJointApplicator : ComponentApplicator {
    public bool applyBreakForce;
    public FloatReference breakForce;
    public bool applyBreakTorque;
    public FloatReference breakTorque;

    public bool motor = true;
    public FloatReference motorMaxTorque;
    public FloatReference motorMaxSpeed;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add fixed joint component to target
        var joint = rigidbodyGo.AddComponent<HingeJoint>();

        // add motor
        var applyMotor = motor && (motorMaxTorque != null) && (motorMaxSpeed != null);
        if (applyMotor) {
            var actuator = rigidbodyGo.AddComponent<SpinnerActuator>();
            actuator.maxTorque = motorMaxTorque;
            actuator.maxSpeed = motorMaxSpeed;
        }

        // apply break limits, as specified
        if (applyBreakForce && breakForce != null) {
            joint.breakForce = breakForce.Value;
        }
        if (applyBreakTorque && breakTorque != null) {
            joint.breakTorque = breakTorque.Value;
        }

        // apply hinge properties
        // FIXME: this needs to be configurable
        joint.axis = new Vector3(0,1,0);

        // add simple joiner script to target, allowing quick joint join
        var joiner = target.AddComponent<Joiner>();
        joiner.joint = joint;
    }
}
