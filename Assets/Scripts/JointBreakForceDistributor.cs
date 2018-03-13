using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakForceDistributor : MonoBehaviour {
    public float breakForce = 1f;
    public float breakTorque = 1f;

    private JointBreakForceApplicator[] applicators;
    private bool firstUpdate = true;

    void Start() {
        applicators = GetComponentsInChildren<JointBreakForceApplicator>();
        Debug.Log("# applicators: " + applicators.Length);
    }


    void SetBreakForces() {
        if (applicators == null) return;
        for (var i=0; i<applicators.Length; i++) {
            applicators[i].breakForce = breakForce;
            applicators[i].breakTorque = breakTorque;
        }
    }

    void Update() {
        if (firstUpdate) {
            SetBreakForces();
            firstUpdate = false;
        }
    }

}
