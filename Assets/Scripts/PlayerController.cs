using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Movement moveScript;

	// Use this for initialization
	void Start () {
        moveScript = gameObject.GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
        moveScript.forwardDrive = Input.GetAxisRaw("Vertical");
        moveScript.rotateDrive = Input.GetAxisRaw("Horizontal");

    }
}
