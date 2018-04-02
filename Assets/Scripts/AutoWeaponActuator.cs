using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// acts as a drive controller repeater.  Expects to take input from a bot controller or AI controller and applies
// drive properties appropriately to child objects (wheel components)
// automatically finds IMovement children
public class AutoWeaponActuator : MonoBehaviour, IActuator {

    public float actuate {
        get {
            return _actuate;
        }
        set {
            _actuate = value;
        }
    }

    private float _actuate = 0.0f;
    private bool sentLastUpdate;
    private bool runDiscovery = true;
    private IActuator[] actuators;

    void Discover() {
        actuators = PartUtil.GetComponentsInChildren<IActuator>(gameObject);
        Debug.Log("# actuators: " + actuators.Length);
    }

	// Update is called once per frame
	void Update () {
        if (runDiscovery) {
            Discover();
            runDiscovery = false;
        }
        if (actuators == null) return;
        // don't spam updates if controller is zero'd
        bool send = false;
        if (!Mathf.Approximately(_actuate, 0f) || !sentLastUpdate) {
            send = true;
        }

        if (send) {
            for (var i=0; i<actuators.Length; i++) {
                actuators[i].actuate = _actuate;
                sentLastUpdate = true;
            }
        } else {
            sentLastUpdate = false;
        }
    }
}
