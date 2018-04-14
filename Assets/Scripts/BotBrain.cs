using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Simple Bot Drive Controller
// Requires IMovement component in same game object
public class BotBrain : MonoBehaviour {
    protected IMovement mover;
    protected IActuator weapon;

    public bool controlsActive = true;

	// Use this for initialization
	void Start () {
        mover = GetComponent<IMovement>();
        weapon = GetComponent<IActuator>();
	}

}
