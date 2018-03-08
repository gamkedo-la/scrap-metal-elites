using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeMotorActuator : MonoBehaviour {
    private bool actuate;

	void Update () {
		actuate = Input.GetKey(KeyCode.U);
	}

    public void FixedUpdate() {
        var hinge = GetComponent<HingeJoint>();
        if (hinge != null) {
            if (actuate) {
                if (hinge.motor.targetVelocity != Mathf.Abs(hinge.motor.targetVelocity)) {
                    JointMotor motor = hinge.motor;
                    motor.force = hinge.motor.force;
                    motor.targetVelocity = Mathf.Abs(hinge.motor.targetVelocity);
                    motor.freeSpin = hinge.motor.freeSpin;
                    hinge.motor = motor;
                }
                hinge.useMotor = true;
            } else if (Mathf.Approximately(transform.localEulerAngles.y, hinge.limits.min)) {
                hinge.useMotor = false;
            } else {
                if (hinge.motor.targetVelocity != -Mathf.Abs(hinge.motor.targetVelocity)) {
                    JointMotor motor = hinge.motor;
                    motor.force = hinge.motor.force;
                    motor.targetVelocity = -Mathf.Abs(hinge.motor.targetVelocity);
                    motor.freeSpin = hinge.motor.freeSpin;
                    hinge.motor = motor;
                }
                hinge.useMotor = true;
            }
        }
    }

}
