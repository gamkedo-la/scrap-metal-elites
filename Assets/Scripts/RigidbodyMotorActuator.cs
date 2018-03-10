using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMotorActuator : MonoBehaviour, IMovement {
    public float forwardDrive {
        get {
            return _forwardDrive;
        }
        set {
            _forwardDrive = value;
        }
    }
    public float rotateDrive {
        get {
            return _rotateDrive;
        }
        set {
            _rotateDrive = value;
        }
    }

    private Rigidbody rb;
    private DriveConfig config;
    private float _forwardDrive = 0.0f;
    private float _rotateDrive = 0.0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        config = GetComponentInParent<DriveConfig>();
    }

    void rbMotor() {
        if (rb == null || config == null) return;
        var f = config.motorMaxTorque * _forwardDrive;
        if (config.motorLeft) {
            f = -f;
        }
        rb.maxAngularVelocity = config.motorMaxSpeed;
        rb.AddTorque(transform.right * f);
    }

    public void FixedUpdate() {
        if (config == null) return;
        if (config.motor) {
            rbMotor();
        }
    }

}
