using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDamageActuator : MonoBehaviour {
    // allow break force/torque to be adjusted based on damage?
    public bool applyDamageToForce = true;
    public bool applyDamageToTorque = true;
    public bool debug = false;

    private Health health;
    private Joint[] joints;
    private float[] maxBreakForces;
    private float[] maxBreakTorques;

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
