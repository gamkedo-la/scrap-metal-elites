using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DriveConfig : MonoBehaviour {
    [System.Serializable]
    public class OnConfigChangeEvent : UnityEvent<DriveConfig> { };

    // steering variables
    public bool steering = false;
    public bool steeringReverse = false;
    public float steeringMaxAngle = 45f;
    public float steeringMaxTorque = 300f;
    public float steeringDamper = 50;

    // motor variables
    public bool motor = false;
    public bool motorLeft = true;
    public float motorMaxSpeed = 100f;
    public float motorMaxTorque = 50;
    public float motorDamper = 50;

    public OnConfigChangeEvent onConfigChange;
    private bool configModified = false;

    void Awake() {
        onConfigChange = new OnConfigChangeEvent();
    }

    void OnValidate() {
        configModified = true;
    }

    void SetConfig() {
        onConfigChange.Invoke(this);
        configModified = false;
    }

    void Start() {
        SetConfig();
    }

    void Update() {
        if (configModified) {
            SetConfig();
        }
    }
}
