using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCalculator : MonoBehaviour {
    void Start() {
        float totalMass = 0;
        var rbs = GetComponentsInChildren<Rigidbody>();
        for (var i=0; i<rbs.Length; i++) {
            //Debug.Log(rbs[i].gameObject.name + " mass " + rbs[i].mass);
            totalMass += rbs[i].mass;
        }

        Debug.Log(gameObject.name + " # rbs: " + rbs.Length + " totalMass: " + totalMass);
    }
}
