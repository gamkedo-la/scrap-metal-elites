using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamActuator : MonoBehaviour, IActuator, IImpactDamageModifier {
    public float actuate {
        get {
            return _actuate;
        }
        set {
            _actuate = value;
        }
    }

    public float impactMultiplier {
        get {
            return impactDamageMultiplier;
        }
    }

    public float power {
        get {
            if (rb != null && maxRamSpeed>0f) {
                return rb.velocity.magnitude/maxRamSpeed;
            } else {
                return 0f;
            }
        }
    }

    private Rigidbody rb;
    private float _actuate = 0.0f;
    private Vector3 minPosition;
    private Vector3 maxPosition;
    private ConfigurableJoint joint;
    public float maxTraverse;
    public float motorForce;
    public float impactForce;
    public float impactDamageMultiplier = 1f;
    public float maxRamSpeed;
    public int axis;
    public bool debug;

    void Start() {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();
        if (axis == 2) {
            minPosition = new Vector3(0,0,-maxTraverse);
            maxPosition = new Vector3(0,0,maxTraverse);
        } else if (axis == 1) {
            minPosition = new Vector3(0,-maxTraverse,0);
            maxPosition = new Vector3(0,maxTraverse,0);
        } else {
            minPosition = new Vector3(-maxTraverse,0,0);
            maxPosition = new Vector3(maxTraverse,0,0);
        }
    }

    void jointMotor() {
        if (joint == null) return;

        // activate
        if (!Mathf.Approximately(_actuate, 0)) {
            joint.targetPosition = minPosition;

        // deactivate
        } else {
            joint.targetPosition = maxPosition;
        }
    }

    public void FixedUpdate() {
        jointMotor();
    }

	void OnCollisionEnter(Collision coll) {
		if(impactForce > 0f) {
            // apply collision force if we hit a rigidbody
            var rbColl = coll.rigidbody;
            if (rbColl != null) {
                // don't apply collision force to self
                if (PartUtil.GetRootGo(gameObject) != PartUtil.GetRootGo(coll.gameObject)) {
                    var f = -coll.contacts[0].normal * impactForce * power;
                    if (debug) {
                        Debug.Log("Ram Collision" +
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
