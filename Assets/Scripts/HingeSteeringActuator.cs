using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeSteeringActuator : MonoBehaviour, IMovement {
    public bool steer = false;
    public bool reverseSteering = false;
    public float steeringSpring = 10;
    public float steeringDamper = 3;
    public float maxSteeringAngle = 30;
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

    private bool configModified = false;
    private float _forwardDrive = 0.0f;
    private float _rotateDrive = 0.0f;
    private HingeJoint joint;

    void OnValidate() {
        configModified = true;
    }

    void SetConfig() {
        if (joint == null) return;
        configModified = false;
        // set joint limits bsaed on public vars
        var hingeSpring = joint.spring;
        hingeSpring.spring = steeringSpring;
        hingeSpring.damper = steeringDamper;
        hingeSpring.targetPosition = 0;
        joint.spring = hingeSpring;
        joint.useSpring = true;
    }

    void Start() {
        joint = GetComponent<HingeJoint>();
        SetConfig();
    }

    void hingeSteer() {
        if (joint == null) return;
        float targetSteeringAngle = 0f;
        if (!Mathf.Approximately(_rotateDrive, 0)) {
            targetSteeringAngle = _rotateDrive * maxSteeringAngle;
            if (reverseSteering) {
                targetSteeringAngle = -targetSteeringAngle;
            }
        }
        if (!Mathf.Approximately(targetSteeringAngle, joint.spring.targetPosition)) {
            var hingeSpring = joint.spring;
            hingeSpring.targetPosition = targetSteeringAngle;
            joint.spring = hingeSpring;
        }
    }

    public void FixedUpdate() {
        if (configModified) {
            SetConfig();
        }
        if (steer) {
            hingeSteer();
        }

    }

}
