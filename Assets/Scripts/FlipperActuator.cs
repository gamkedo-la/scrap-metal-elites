using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperActuator : MonoBehaviour, IActuator {
    public float actuate {
        get {
            return _actuate;
        }
        set {
            _actuate = value;
        }
    }

    public float power {
        get {
            if (rb != null && maxSpeed > 0) {
                return rb.angularVelocity.magnitude/maxSpeed;
            } else {
                return 0f;
            }
        }
    }

    public float currentAngle {
        get {
            if (axis == 2) {
                return transform.localEulerAngles.z;
            } else if (axis == 1) {
                return transform.localEulerAngles.y;
            } else {
                return transform.localEulerAngles.x;
            }
        }
    }

    public float targetAngle {
        get {
            if (reverse) {
                return maxAngle;
            } else {
                return minAngle;
            }
        }
    }

    private Rigidbody rb;
    private float _actuate = 0.0f;
    private Vector3 torqueVector;

    public float maxTorque;
    public float maxSpeed;
	public float impactForce;
    public int axis;
    public bool useSpring;
    public float minAngle;
    public float maxAngle;
    public bool reverse;

    void Start() {
        rb = GetComponent<Rigidbody>();
        if (axis == 2) {
            torqueVector = transform.forward;
        } else if (axis == 1) {
            torqueVector = transform.up;
        } else {
            torqueVector = transform.right;
        }
    }

    void rbMotor() {
        if (rb == null) return;
        // if we use spring to return body to rest, and zero actuate return
        if (Mathf.Approximately(_actuate, 0) && useSpring) return;
        // if we don't use spring, and we are already at rest position w/ zero actuate return
        if (Mathf.Approximately(_actuate, 0) && (!useSpring && Mathf.Approximately(currentAngle, targetAngle))) return;

        // if at zero actuate (not using spring), reverse force
        float f;
        if (Mathf.Approximately(_actuate, 0)) {
            f = -maxTorque;
        } else {
            f = maxTorque * _actuate;
        }
        if (reverse) {
            f = -f;
        }
        rb.maxAngularVelocity = maxSpeed;
        rb.AddRelativeTorque(torqueVector * f);
    }

    public void FixedUpdate() {
        rbMotor();
    }

	void OnCollisionEnter(Collision coll) {
		if(power > 0f && impactForce > 0f) {
            // apply collision force if we hit a rigidbody
            var rbColl = coll.rigidbody;
            if (rbColl != null) {
                // don't apply collision force to self
                if (PartUtil.GetRootGo(gameObject) != PartUtil.GetRootGo(coll.gameObject)) {
                    var f = -coll.contacts[0].normal * impactForce * power;
    				rbColl.AddForceAtPosition(f, coll.contacts[0].point);
                }
            }
		}
	}

}
