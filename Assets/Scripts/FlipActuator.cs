using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipActuator : MonoBehaviour
{
    public float maxTorque = 30f;
    public float maxSpinSpeed = 750f;
    public float rampUpSpeed = 250f;
    public float rampDownSpeed = 250f;
    public float minAngle = 0f;
    [Range (0f,270f)]
    public float maxAngle = 180f;
    public GameObject shaft;
	private bool actuate = false;
    private float currentSpeed = 0f;
    private Rigidbody rb;

	void Start() {
		//myController = GetComponentInParent<Movement>();
        rb = GetComponent<Rigidbody>();
	}


	void Update ()
    {
		actuate = Input.GetKey(KeyCode.U);
	}

    public void FixedUpdate() {
        if (actuate) {
            if (rb != null) {
                var f = maxTorque * Time.deltaTime;
                rb.AddTorque(transform.up * f);

            }
            /*
            if (shaft.transform.localEulerAngles.y < maxAngle) {
                if (currentSpeed > maxSpinSpeed) {
                    currentSpeed = maxSpinSpeed;
                } else {
                    currentSpeed += rampUpSpeed;
                }
            } else {
                currentSpeed = 0f;
                shaft.transform.localEulerAngles = new Vector3(0f, maxAngle, 0f);
            }
            */
        } else {
            /*
            //if (shaft.transform.localEulerAngles.y > rotMin.y) {
            // euler angles on the transform is always returned as a float between 0 and 360,
            // a negative rotation will not be caught, so detect > max
            var angle = shaft.transform.localEulerAngles.y;
            if ((angle <= maxAngle) && (angle>minAngle)) {
                if (currentSpeed < -maxSpinSpeed) {
                    currentSpeed = -maxSpinSpeed;
                } else {
                    currentSpeed -= rampDownSpeed;
                }
            } else {
                currentSpeed = 0f;
                // reset transform to min angle
                shaft.transform.localEulerAngles = new Vector3(0f, minAngle, 0f);
            }
            */
        }
        /*
        if (!Mathf.Approximately(currentSpeed, 0f)) {
            shaft.transform.Rotate(Vector3.up * currentSpeed * Time.deltaTime);
        }
        */
    }

}
