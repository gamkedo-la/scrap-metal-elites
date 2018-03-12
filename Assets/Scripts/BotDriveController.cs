using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Simple Bot Drive Controller
// Requires IMovement component in same game object
public class BotDriveController : MonoBehaviour {
    private IMovement moveScript;

	// Use this for initialization
	void Start () {
        moveScript = GetComponent<IMovement>();
	}

	// Update is called once per frame
	void Update () {
        if (moveScript != null) {
            moveScript.forwardDrive = Input.GetAxis("Vertical");
            moveScript.rotateDrive = Input.GetAxis("Horizontal");
        }
    }
}
