using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakForceApplicator : MonoBehaviour {
    public float breakForceMultiplier = 1f;
    public float breakTorqueMultiplier = 1f;

    public float breakForce {
        set {
            if (joints != null) {
                for (var i=0; i<joints.Length; i++) {
                    joints[i].breakForce = value * breakForceMultiplier;
                }
            }
        }
    }
    public float breakTorque {
        set {
            if (joints != null) {
                for (var i=0; i<joints.Length; i++) {
                    joints[i].breakTorque = value * breakTorqueMultiplier;
                }
            }
        }
    }

    private Joint[] joints;

    void Start() {
        joints = GetComponents<Joint>();
    }

}
