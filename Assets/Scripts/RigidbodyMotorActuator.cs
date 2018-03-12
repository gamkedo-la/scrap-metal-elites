using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMotorActuator : MonoBehaviour, IMotorActuator {
    public float forwardDrive {
        get {
            return _forwardDrive;
        }
        set {
            _forwardDrive = value;
        }
    }
    public bool left {
        get {
            if (config != null) {
                return config.motorLeft;
            } else {
                return false;
            }
        }
    }

    private Rigidbody rb;
    private DriveConfig config;
    private float _forwardDrive = 0.0f;

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
