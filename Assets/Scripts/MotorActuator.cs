using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorActuator : MonoBehaviour, IMotorActuator {
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
            return isLeft;
        }
    }

    private Rigidbody rb;
    private float _forwardDrive = 0.0f;
    public bool isLeft = false;
    public FloatReference maxTorque;
    public FloatReference maxSpeed;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void rbMotor() {
        if (rb == null || maxTorque == null || maxSpeed == null) return;
        if (Mathf.Approximately(_forwardDrive, 0)) {
            return;
        }
        var f = maxTorque.Value * _forwardDrive;
        if (isLeft) {
            f = -f;
        }
        rb.maxAngularVelocity = maxSpeed.Value;
        rb.AddTorque(transform.right * f);
    }

    public void FixedUpdate() {
        rbMotor();
    }

}
