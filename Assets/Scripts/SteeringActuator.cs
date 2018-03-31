using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringActuator : MonoBehaviour, ISteeringActuator {
    public float rotateDrive {
        get {
            return _rotateDrive;
        }
        set {
            _rotateDrive = value;
        }
    }

    private float _rotateDrive = 0.0f;
    private HingeJoint joint;
    public bool reverse = false;
    public float maxTurnAngle = 0;

    void Start() {
        joint = GetComponent<HingeJoint>();
    }

    void hingeSteer() {
        if (joint == null) return;
        float targetSteeringAngle = 0f;
        if (!Mathf.Approximately(_rotateDrive, 0)) {
            targetSteeringAngle = _rotateDrive * maxTurnAngle;
            if (reverse) {
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
        hingeSteer();
    }

}
