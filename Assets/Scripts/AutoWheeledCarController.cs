using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoWheeledCarController : MonoBehaviour {

    public IMovement[] moveScripts;

	// Use this for initialization
	void Start () {
        moveScripts = GetComponentsInChildren<IMovement>();
        print("# moveScripts: " + moveScripts.Length);
	}

	// Update is called once per frame
	void Update () {
        var forwardDrive = Input.GetAxis("Vertical");
        var rotateDrive = Input.GetAxis("Horizontal");
        //if (!Mathf.Approximately(forwardDrive, 0f) || !Mathf.Approximately(forwardDrive, 0f)) {
            for (var i=0; i<moveScripts.Length; i++) {
                moveScripts[i].forwardDrive = forwardDrive;
                moveScripts[i].rotateDrive = rotateDrive;
            }
        //}

    }
}
