using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakForceApplicator : MonoBehaviour {
    public float breakForceMultiplier = 1f;
    public float breakTorqueMultiplier = 1f;
    // allow break force/torque to be pushed?
    public bool applyDistributedForce = true;
    public bool applyDistributedTorque = true;
    // allow break force/torque to be adjusted based on damage?
    public bool applyDamageToForce = true;
    public bool applyDamageToTorque = true;
    public bool debug = false;

    private Health health;
    private Joint[] joints;
    private float[] maxBreakForces;
    private float[] maxBreakTorques;

    public float breakForce {
        set {
            if (joints != null && applyDistributedForce) {
                var force = value * breakForceMultiplier;
                for (var i=0; i<joints.Length; i++) {
                    if (joints[i] != null) joints[i].breakForce = force;
                    maxBreakForces[i] = force;
                }
            }
        }
    }
    public float breakTorque {
        set {
            if (joints != null && applyDistributedTorque) {
                var force = value * breakTorqueMultiplier;
                for (var i=0; i<joints.Length; i++) {
                    if (joints[i] != null) joints[i].breakTorque = force;
                    maxBreakTorques[i] = force;
                }
            }
        }
    }


    void Start() {
        joints = GetComponents<Joint>();
        maxBreakForces = new float[joints.Length];
        maxBreakTorques = new float[joints.Length];
        // initialize max break values based on assigned values...
        for (var i=0; i<joints.Length; i++) {
            maxBreakForces[i] = joints[i].breakForce;
            maxBreakTorques[i] = joints[i].breakTorque;
        }
        health = GetComponent<Health>();
        if (health != null) {
            if (applyDamageToForce || applyDamageToTorque) {
                health.onChangePercent.AddListener(OnHealthPercentChange);
            }
        }
    }

    void OnHealthPercentChange(int newValue) {
        Debug.Log("OnHealthPercentChange: " + newValue);
        if (joints == null) return;
        if (applyDamageToForce) {
            for (var i=0; i<joints.Length; i++) {
                if (joints[i] != null) {
                    var force = ((float) newValue * maxBreakForces[i])/100f;
                    if (debug) Debug.Log("JointBreakForceApplicator: setting break force from " + joints[i].breakForce + " to: " + force);
                    joints[i].breakForce = force;
                }
            }
        }
        if (applyDamageToTorque) {
            for (var i=0; i<joints.Length; i++) {
                if (joints[i] != null) {
                    var force = ((float) newValue * maxBreakTorques[i])/100f;
                    if (debug) Debug.Log("JointBreakForceApplicator: setting break torque from " + joints[i].breakTorque + " to: " + force);
                    joints[i].breakTorque = force;
                }
            }
        }
    }

}
