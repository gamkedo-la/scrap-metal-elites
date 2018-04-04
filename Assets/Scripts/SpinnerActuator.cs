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

    public float spinPower {
        get {
            if (rb != null && maxSpeed > 0) {
                return rb.angularVelocity.magnitude/maxSpeed;
            } else {
                return 0f;
            }
        }
    }

    private Rigidbody rb;
    private float _actuate = 0.0f;
    public float maxTorque;
    public float maxSpeed;
	public float throwForceLateral = 400.0f;
	public float throwForceUpward = 400.0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void rbMotor() {
        if (rb == null) return;
        if (Mathf.Approximately(_actuate, 0)) return;
        var f = maxTorque * _actuate;
        rb.maxAngularVelocity = maxSpeed;
        // FIXME: axis configurable?
        rb.AddTorque(transform.up * f);
    }

    public void FixedUpdate() {
        rbMotor();
    }

	void OnCollisionEnter(Collision coll) {
		if(spinPower > 0f) {
            // apply collision force if we hit a rigidbody
            var rbColl = coll.rigidbody;
            if (rbColl != null) {
                // don't apply collision force to self
                if (PartUtil.GetRootGo(gameObject) != PartUtil.GetRootGo(coll.gameObject)) {
                    var f = (-transform.right) * throwForceLateral * spinPower + transform.up * throwForceUpward * spinPower;
                    //Debug.Log("applying f: " + f + " to " + rbColl.gameObject.name);
    				rbColl.AddForceAtPosition( f, coll.contacts[0].point);
                }
            }
		}
	}

}
