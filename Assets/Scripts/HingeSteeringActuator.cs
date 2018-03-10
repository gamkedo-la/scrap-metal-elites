using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeSteeringActuator : MonoBehaviour, IMovement {
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
    private HingeJoint joint;
    private DriveConfig config;

    void OnConfigModified(DriveConfig config) {
        if (joint == null || config == null) return;
        // set joint limits/params bsaed on public vars
        var limits = joint.limits;
        limits.min = -config.steeringMaxAngle;
        limits.max = config.steeringMaxAngle;
        joint.limits = limits;
        joint.useLimits = true;
        var hingeSpring = joint.spring;
        hingeSpring.spring = config.steeringMaxTorque;
        hingeSpring.damper = config.steeringDamper;
        hingeSpring.targetPosition = 0;
        joint.spring = hingeSpring;
        joint.useSpring = true;
    }

    void Start() {
        joint = GetComponent<HingeJoint>();
        config = GetComponentInParent<DriveConfig>();
        if (config != null) {
            config.onConfigChange.AddListener(OnConfigModified);
            OnConfigModified(config);
        }
    }

    void hingeSteer() {
        if (joint == null || config == null) return;
        float targetSteeringAngle = 0f;
        if (!Mathf.Approximately(_rotateDrive, 0)) {
            targetSteeringAngle = _rotateDrive * config.steeringMaxAngle;
            if (config.steeringReverse) {
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
        if (config == null) return;
        if (config.steering) {
            hingeSteer();
        }

    }

}
