using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretActuator : MonoBehaviour, ISteeringActuator {
    public float rotateDrive {
        get {
            return _rotateDrive;
        }
        set {
            _rotateDrive = value;
        }
    }

    private HingeJoint joint;
    private float _rotateDrive;

    public float turnSpeed = 10f;
    public float turnForce = 5f;
    public bool debug;

    void Start() {
        joint = GetComponent<HingeJoint>();
        if (joint != null) {
            joint.useMotor = true;
        }
    }

    void hingeMotor() {
        if (joint == null) return;

        var targetVelocity = turnSpeed * _rotateDrive;
        if (!Mathf.Approximately(targetVelocity, joint.motor.targetVelocity)) {
            var motor = joint.motor;
            motor.targetVelocity = targetVelocity;
            motor.force = turnForce;
            joint.motor = motor;
        }
    }

    public void FixedUpdate() {
        hingeMotor();
    }

}
