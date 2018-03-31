using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// acts as a drive controller repeater.  Expects to take input from a bot controller or AI controller and applies
// drive properties appropriately to child objects (wheel components)
// automatically finds IMovement children
public class AutoWheeledMovement : MonoBehaviour, IMovement {

    public float forwardDrive {
        get {
            return _forwardDrive;
        }
        set {
            _forwardDrive = value;
        }
    }
    public float rotateDrive {
        get {
            return _rotateDrive;
        }
        set {
            _rotateDrive = value;
        }
    }

    private float _forwardDrive = 0.0f;
    private float _rotateDrive = 0.0f;
    private bool motorSentLastUpdate;
    private bool steeringSentLastUpdate;
    private bool runDiscovery = true;
    private IMotorActuator[] motorScripts;
    private ISteeringActuator[] steeringScripts;

    void Discover() {
        motorScripts = GetComponentsInChildren<IMotorActuator>();
        steeringScripts = GetComponentsInChildren<ISteeringActuator>();
        Debug.Log("# motorSripts: " + motorScripts.Length + " steeringScripts: " + steeringScripts.Length);
    }

	// Update is called once per frame
	void Update () {
        if (runDiscovery) {
            Discover();
            runDiscovery = false;
        }
        // don't spam updates if controller is zero'd
        bool sendMotor = false;
        if (!Mathf.Approximately(_forwardDrive, 0f) || !motorSentLastUpdate) {
            sendMotor = true;
        }
        bool sendSteering = false;
        if (!Mathf.Approximately(_rotateDrive, 0f) || !steeringSentLastUpdate) {
            sendSteering = true;
        }

        if (sendMotor) {
            for (var i=0; i<motorScripts.Length; i++) {
                motorScripts[i].forwardDrive = _forwardDrive;
                motorSentLastUpdate = true;
            }
        } else {
            motorSentLastUpdate = false;
        }

        if (sendSteering) {
            for (var i=0; i<steeringScripts.Length; i++) {
                steeringScripts[i].rotateDrive = _rotateDrive;
                steeringSentLastUpdate = true;
            }
        } else {
            steeringSentLastUpdate = false;
        }

    }
}
