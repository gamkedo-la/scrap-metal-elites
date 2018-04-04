using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "spinnerJoint", menuName = "Joints/Spinner")]
public class SpinnerJointApplicator : JointApplicator {
    public bool motor = true;
    public float motorMaxTorque;
    public float motorMaxSpeed;
	public float throwForceLateral = 400.0f;
	public float throwForceUpward = 400.0f;

    protected override Joint ApplyJoint(GameObject rigidbodyGo, PartConfig config, GameObject target) {

        // add fixed joint component to target
        var joint = rigidbodyGo.AddComponent<HingeJoint>();
        // apply hinge properties
        // FIXME: this needs to be configurable
        joint.axis = new Vector3(0,1,0);

        // add motor
        var applyMotor = motor && (motorMaxTorque != null) && (motorMaxSpeed != null);
        if (applyMotor) {
            var actuator = rigidbodyGo.AddComponent<SpinnerActuator>();
            actuator.maxTorque = motorMaxTorque;
            actuator.maxSpeed = motorMaxSpeed;
            actuator.throwForceUpward = throwForceUpward;
            actuator.throwForceLateral = throwForceLateral;
        }
        return joint;

    }
}
