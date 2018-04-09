using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCalculator : MonoBehaviour {
    private bool runDiscover = true;
    void Discover() {
        float totalMass = 0;
        var rbs = PartUtil.GetComponentsInChildren<Rigidbody>(gameObject);
        for (var i=0; i<rbs.Length; i++) {
            totalMass += rbs[i].mass;
        }
        Debug.Log(gameObject.name + " # rbs: " + rbs.Length + " totalMass: " + totalMass);
    }

    void Update() {
        if (runDiscover) {
            Discover();
            runDiscover = false;
        }
    }
}
