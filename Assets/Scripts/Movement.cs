using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IMovement {

    public float moveTopSpeed = 5.0f;
    public float turnSpeed = 2.5f;
    public float accelSpeed = 3.0f;
    public float friction = 0.1f;
    private float moveSpeed = 0.0f;
    private float _forwardDrive = 0.0f;
    private float _rotateDrive = 0.0f;

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

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        moveSpeed += Time.deltaTime * accelSpeed * forwardDrive;
        moveSpeed = Mathf.Clamp(moveSpeed, -moveTopSpeed, moveTopSpeed);
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
		transform.Rotate(Vector3.up, (moveSpeed>0?1:-1) * turnSpeed * Time.deltaTime * rotateDrive); // like a car, turn the opposite way when reversing
    }

    private void FixedUpdate()
    {
        moveSpeed *= (1.0f - friction);
    }

}
