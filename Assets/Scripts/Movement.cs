using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float moveTopSpeed = 5.0f;
    public float turnSpeed = 2.5f;
    public float accelSpeed = 3.0f;
    public float friction = 0.1f;
    private float moveSpeed = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        moveSpeed += Time.deltaTime * accelSpeed * Input.GetAxisRaw("Vertical");
        moveSpeed = Mathf.Clamp(moveSpeed, -moveTopSpeed, moveTopSpeed);
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
    }

    private void FixedUpdate()
    {
        moveSpeed *= (1.0f - friction);
    }

}
