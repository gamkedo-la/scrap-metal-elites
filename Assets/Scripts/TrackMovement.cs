using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TrackAxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
}

public class TrackMovement : MonoBehaviour, IMovement {
    public List<TrackAxleInfo> axleInfos;
    public float maxMotorTorque;

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

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        float leftMotor = maxMotorTorque * (forwardDrive + rotateDrive)/2f;
        float rightMotor = maxMotorTorque * (forwardDrive - rotateDrive)/2f;

        foreach (var axleInfo in axleInfos) {
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = leftMotor;
                axleInfo.rightWheel.motorTorque = rightMotor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}
