using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySteeringActuator : MonoBehaviour, IMovement {
    public bool steer = false;
    public bool reverseSteering = false;
    public float steeringTorque = 10;
    public float steeringSlough = 3;
    public float maxSteeringAngle = 30;

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
    // NOT IMPLEMENTED
    public bool left {
        get {
            return false;
        }
    }

    private float _forwardDrive = 0.0f;
    private float _rotateDrive = 0.0f;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void rbSteer() {
        if (rb == null) return;

        /*
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x = 0;
        localVelocity.y = 0;
        localVelocity.z = 0;
        rb.velocity = transform.TransformDirection(localVelocity);

         Vector3 target = Vector3.zero;
         Vector3 distance = target - transform.position;
         Vector3 idealPosition = target - transform.forward * distance.magnitude;
         Vector3 correction = idealPosition - transform.position;

         correction = transform.InverseTransformDirection( correction );
         //correction.x = 0;
         //correction.y = 0;
         //correction.z = 0;
         correction = transform.TransformDirection( correction );

         rb.velocity += correction * 25f;
         */

        float targetSteeringAngle = 0f;
        if (!Mathf.Approximately(_rotateDrive, 0)) {
            targetSteeringAngle = _rotateDrive * maxSteeringAngle;
            if (!reverseSteering) {
                targetSteeringAngle = -targetSteeringAngle;
            }
        }
        var f = steeringTorque;
        var normalizedEulerAngle = (transform.localEulerAngles.y>180f) ? transform.localEulerAngles.y - 360f : transform.localEulerAngles.y;
        if (Mathf.Abs(targetSteeringAngle - normalizedEulerAngle) > steeringSlough) {
            if (targetSteeringAngle < normalizedEulerAngle) {
                f = -steeringTorque;
            }
            print("targetSteeringAngle: " + targetSteeringAngle + " lea: " + transform.localEulerAngles.y + " f: " + f);
            rb.AddTorque(transform.up * f);
        }
    }

    public void FixedUpdate() {
        if (steer) {
            rbSteer();
        }
    }

}
