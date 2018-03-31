using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Simple Bot Drive Controller
// Requires IMovement component in same game object
public class ActuatorController : MonoBehaviour {
    private IActuator actuator;

	// Use this for initialization
	void Start () {
        actuator = GetComponent<IActuator>();
	}

	// Update is called once per frame
	void Update () {
        if (actuator != null) {
    		actuator.actuate = (Input.GetKey(KeyCode.Alpha1)) ? 1f : 0f;
        }
    }
}
