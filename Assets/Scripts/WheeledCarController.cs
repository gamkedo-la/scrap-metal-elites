using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheeledCarController : MonoBehaviour {

    public IMovement moveScript;

	// Use this for initialization
	void Start () {
        moveScript = gameObject.GetComponent<IMovement>();
	}

	// Update is called once per frame
	void Update () {
        moveScript.forwardDrive = Input.GetAxis("Vertical");
        moveScript.rotateDrive = Input.GetAxis("Horizontal");

    }
}
