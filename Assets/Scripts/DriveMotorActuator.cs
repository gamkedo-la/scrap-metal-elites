using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveMotorActuator : MonoBehaviour, IMovement {
    public bool steer = false;
    public bool motor = false;
    public bool left = true;
    public bool reverseSteering = false;
    public float maxSpeed = 100f;
    public float steeringSpring = 300f;
    public float steeringDamper = 50;
    public float maxTorque = 50;
    public float yDamper = 1;
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

    private bool configModified = false;
    private float _forwardDrive = 0.0f;
    private float _rotateDrive = 0.0f;
    private float targetFlexRotation = 0f;
    private ConfigurableJoint joint;

    void OnValidate() {
        configModified = true;
    }

    void SetConfig() {
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
        // motor drive (local Y axis is X axis)
        var yDrive = new JointDrive();
        yDrive.maximumForce = maxTorque;
        yDrive.positionDamper = yDamper;
        yDrive.positionSpring = 0f;
        joint.angularYZDrive = yDrive;
        configModified = false;
    }

    void Start() {
        joint = GetComponent<ConfigurableJoint>();

        targetFlexRotation = 1f - maxFlexAngle/maxSteeringAngle;
        SetConfig();
    }

    public void FixedUpdate() {
        if (configModified) {
            SetConfig();
        }

        //var hinge = GetComponent<HingeJoint>();
        var joint = GetComponent<ConfigurableJoint>();
        if (joint == null) return;

        if (motor) {
            float targetSpeed = 0f;
            if (!Mathf.Approximately(forwardDrive, 0)) {
                targetSpeed = maxSpeed;
                // reverse motor direction for opposite tires
                if (left) {
                    targetSpeed *= (-1f * _forwardDrive);
                } else {
                    targetSpeed *= _forwardDrive;
                }
            }
            if (joint.targetAngularVelocity.y != targetSpeed) {
                // if gas is applied, ensure yDrive is set to max torque
                if (Mathf.Abs(targetSpeed) > .01f && !Mathf.Approximately(joint.angularYZDrive.maximumForce, maxTorque)) {
                    var yDrive = new JointDrive();
                    yDrive.maximumForce = maxTorque;
                    yDrive.positionDamper = yDamper;
                    yDrive.positionSpring = 0f;
                    joint.angularYZDrive = yDrive;
                // if gas is let up, allow free speen of wheels
                } else if (targetSpeed < .01f && !Mathf.Approximately(joint.angularYZDrive.maximumForce, 0f)) {
                    var yDrive = new JointDrive();
                    yDrive.maximumForce = 0;
                    yDrive.positionDamper = yDamper;
                    yDrive.positionSpring = 0f;
                    joint.angularYZDrive = yDrive;
                }
                // set target angular velocity
                joint.targetAngularVelocity = new Vector3(0f, targetSpeed, 0f);
            }
        }

        /*
        if (steer) {
            float targetRotation;
            if (reverseSteering) {
                targetRotation = _rotateDrive;
            } else {
                targetRotation = -_rotateDrive;
            }
            joint.targetRotation = new Quaternion(targetRotation,0,0,1);
        }
        */

        if (steer) {
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

    }

}
