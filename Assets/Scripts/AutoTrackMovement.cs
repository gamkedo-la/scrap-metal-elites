using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoTrackMovement : MonoBehaviour, IMovement {
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
    // top-level movement repeater, left/right doesn't make sense at this level
    public bool left {
        get {
            return false;
        }
    }

    private float _forwardDrive = 0.0f;
    // NOT USED
    private float _rotateDrive = 0.0f;
    private bool runDiscovery = true;
    private bool leftSentLastUpdate;
    private bool rightSentLastUpdate;
    private List<IMotorActuator> leftScripts;
    private List<IMotorActuator> rightScripts;

    void Discover() {
        leftScripts = new List<IMotorActuator>();
        rightScripts = new List<IMotorActuator>();
        var scripts = PartUtil.GetComponentsInChildren<IMotorActuator>(gameObject);
        for (var i=0; i<scripts.Length; i++) {
            if (scripts[i].left) {
                leftScripts.Add(scripts[i]);
            } else {
                rightScripts.Add(scripts[i]);
            }
        }
    }

    void Update() {
        if (runDiscovery) {
            Discover();
            runDiscovery = false;
        }
    }

    public void FixedUpdate() {
        if ((leftScripts == null) || (rightScripts == null)) return;
        float leftDrive = Mathf.Clamp(_forwardDrive + (_rotateDrive*1.25f), -1f, 1f);
        float rightDrive = Mathf.Clamp(_forwardDrive - (_rotateDrive*1.25f), -1f, 1f);

        // don't spam updates if controller is zero'd
        bool sendLeft = false;
        if (!Mathf.Approximately(leftDrive, 0f) || !leftSentLastUpdate) {
            sendLeft = true;
        }
        bool sendRight = false;
        if (!Mathf.Approximately(rightDrive, 0f) || !rightSentLastUpdate) {
            sendRight = true;
        }

        if (sendLeft) {
            for (var i=0; i<leftScripts.Count; i++) {
                leftScripts[i].forwardDrive = leftDrive;
                leftSentLastUpdate = true;
            }
        }
        if (!Mathf.Approximately(leftDrive, 0f)) {
            leftSentLastUpdate = false;
        }

        if (sendRight) {
            for (var i=0; i<rightScripts.Count; i++) {
                rightScripts[i].forwardDrive = rightDrive;
                rightSentLastUpdate = true;
            }
        }
        if (!Mathf.Approximately(rightDrive, 0f)) {
            rightSentLastUpdate = false;
        }

    }
}
