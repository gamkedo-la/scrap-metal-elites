using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSpin : MonoBehaviour
{
    public float spinSpeed = 5.0f;
	public float throwForceLateral = 400.0f;
	public float throwForceUpward = 400.0f;
	private bool bladeSpinning = false;
	private Movement myController;
    
	void Start() {
		myController = GetComponentInParent<Movement>();
	}

	void Update ()
    {
		bladeSpinning = Input.GetButton("Fire1"); // saving for use in collision state, too
		if (bladeSpinning)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * -spinSpeed);
        }
	}

	void OnCollisionEnter(Collision coll)
	{
		if(bladeSpinning) {
			Movement otherController = coll.gameObject.GetComponentInParent<Movement>();
			if(otherController != myController) { // blade hitting someone else?
				Rigidbody rb = otherController.gameObject.GetComponent<Rigidbody>();
				rb.AddForceAtPosition( -myController.transform.right * throwForceLateral +
					transform.up * throwForceUpward, coll.contacts[0].point);
			}
		}
	}
}
