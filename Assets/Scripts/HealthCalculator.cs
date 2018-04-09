using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCalculator : MonoBehaviour {
    private bool runDiscover = true;
    void Discover() {
        int totalHealth = 0;
        var components = PartUtil.GetComponentsInChildren<Health>(gameObject);
        for (var i=0; i<components.Length; i++) {
            totalHealth += components[i].maxHealth;
        }
        Debug.Log(gameObject.name + " # components: " + components.Length + " totalHealth: " + totalHealth);
    }

    void Update() {
        if (runDiscover) {
            Discover();
            runDiscover = false;
        }
    }
}
