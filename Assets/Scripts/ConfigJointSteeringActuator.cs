using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigJointSteeringActuator : MonoBehaviour, IMovement {
    public bool steer = false;
    public bool reverseSteering = false;
    public float steeringSpring = 300f;
    public float steeringDamper = 50;
    public float maxSteeringAngle = 45f;
    public float maxFlexAngle = 1f;

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
    // NOT IMPLEMENTED FOR STEERING CONTROLLER
    public bool left {
        get {
            return false;
        }
    }

    private bool configModified = false;
    private float _forwardDrive = 0.0f;
    private float _rotateDrive = 0.0f;
    private float targetFlexRotation = 0f;
    private ConfigurableJoint joint;

    void OnValidate() {
        configModified = true;
    }

    void SetConfig() {
        if (joint == null) return;
        // set joint limits bsaed on public vars
        var lowLimit = new SoftJointLimit();
        //lowLimit.limit = -maxFlexAngle;
        lowLimit.limit = -maxSteeringAngle;
        joint.lowAngularXLimit = lowLimit;
        var highLimit = new SoftJointLimit();
        //highLimit.limit = maxFlexAngle;
        highLimit.limit = maxSteeringAngle;
        joint.highAngularXLimit = highLimit;
        // steering drive (local X axis is object Y axis)
        var xDrive = new JointDrive();
        xDrive.positionSpring = steeringSpring;
        xDrive.positionDamper = steeringDamper;
        xDrive.maximumForce = joint.angularXDrive.maximumForce;
        joint.angularXDrive = xDrive;
        if (steer) {
            joint.angularXMotion = ConfigurableJointMotion.Limited;
        } else {
            joint.angularXMotion = ConfigurableJointMotion.Locked;
        }
        configModified = false;
    }

    void Start() {
        joint = GetComponent<ConfigurableJoint>();

        targetFlexRotation = 1f - maxFlexAngle/maxSteeringAngle;
        SetConfig();
    }

    void cjSteer() {
        var joint = GetComponent<ConfigurableJoint>();
        if (joint == null) return;
        float targetSteeringAngle = 0f;
        float targetRotation = 0;
        if (!Mathf.Approximately(_rotateDrive, 0)) {
            targetSteeringAngle = _rotateDrive * maxSteeringAngle;
            if (_rotateDrive < 0f) {
                targetRotation = -targetFlexRotation;
            } else {
                targetRotation = targetFlexRotation;
            }
            if (!reverseSteering) {
                targetSteeringAngle = -targetSteeringAngle;
                targetRotation = -targetRotation;
            }
        }
        if (!Mathf.Approximately(targetSteeringAngle, (joint.highAngularXLimit.limit + joint.lowAngularXLimit.limit)/2f)) {
            var lowLimit = new SoftJointLimit();
            lowLimit.limit = targetSteeringAngle-maxFlexAngle;
            joint.lowAngularXLimit = lowLimit;
            var highLimit = new SoftJointLimit();
            highLimit.limit = targetSteeringAngle+maxFlexAngle;
            joint.highAngularXLimit = highLimit;
        }
        if (!Mathf.Approximately(joint.targetRotation.x, targetRotation)) {
            joint.targetRotation = new Quaternion(targetRotation,0,0,1);
        }
    }

    void cjSteer2() {
        float targetRotation;
        if (reverseSteering) {
            targetRotation = _rotateDrive;
        } else {
            targetRotation = -_rotateDrive;
        }
        joint.targetRotation = new Quaternion(targetRotation,0,0,1);
    }

    public void FixedUpdate() {
        if (configModified) {
            SetConfig();
        }

        if (steer) {
            cjSteer2();
        }

    }

}
