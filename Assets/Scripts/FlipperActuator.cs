using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperActuator : MonoBehaviour, IActuator, IImpactDamageModifier {
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
            if (rb != null) {
                return rb.angularVelocity.magnitude*impactForceVelocityMultiplier;
            } else {
                return 0f;
            }
        }
    }


    public float impactMultiplier {
        get {
            return impactDamageMultiplier;
        }
    }

    private Rigidbody rb;
    private HingeJoint joint;
    private float _actuate = 0.0f;

	public float impactForce;
	public float impactForceVelocityMultiplier;
    public float impactDamageMultiplier;
    public float minAngle;
    public float maxAngle;
    public bool reverse;
    public bool debug;

    void Start() {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
    }

    void hingeMotor() {
        if (joint == null) return;

        // activate
        if (!Mathf.Approximately(_actuate, 0)) {
            var hingeSpring = joint.spring;
            hingeSpring.targetPosition = (reverse) ? minAngle : maxAngle;
            joint.spring=hingeSpring;

        // deactivate
        } else {
            var hingeSpring = joint.spring;
            hingeSpring.targetPosition = (reverse) ? maxAngle : minAngle;
            joint.spring=hingeSpring;
        }
    }

    public void FixedUpdate() {
        hingeMotor();
    }

	void OnCollisionEnter(Collision coll) {
		if(power > 0f && impactForce > 0f) {
            // apply collision force if we hit a rigidbody
            var rbColl = coll.rigidbody;
            if (rbColl != null) {
                // don't apply collision force to self
                if (PartUtil.GetRootGo(gameObject) != PartUtil.GetRootGo(coll.gameObject)) {
                    var f = -coll.contacts[0].normal * impactForce * power;
                    if (debug) {
                        Debug.Log("Flipper Collision" +
                                  "\napplying f: " + f + "(" + f.magnitude + ")" +
                                  "\npower: " + power +
                                  "\ncollided with: " + rbColl.gameObject.name);
                    }
    				rbColl.AddForceAtPosition(f, coll.contacts[0].point);
                }
            }
		}
	}

}
