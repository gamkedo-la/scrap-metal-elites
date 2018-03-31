using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerActuator : MonoBehaviour, IActuator {
    public float actuate {
        get {
            return _actuate;
        }
        set {
            _actuate = value;
        }
    }

    private Rigidbody rb;
    private float _actuate = 0.0f;
    public FloatReference maxTorque;
    public FloatReference maxSpeed;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void rbMotor() {
        if (rb == null || maxTorque == null || maxSpeed == null) return;
        if (Mathf.Approximately(_actuate, 0)) return;
        var f = maxTorque.Value * _actuate;
        rb.maxAngularVelocity = maxSpeed.Value;
        // FIXME: configurable?
        rb.AddTorque(transform.up * f);
    }

    public void FixedUpdate() {
        rbMotor();
    }

}
