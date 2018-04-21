using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Simple Bot Drive Controller
// Requires IMovement component in same game object
public class HumanController : BotBrain {
	void Awake() {
		// assign player tag
		var rootGo = PartUtil.GetRootGo(gameObject);
		if (rootGo != null) {
			rootGo.tag = "player";
		}
	}

	// Update is called once per frame
	void Update () {
        // are controls disabled?
        if (!controlsActive) return;

        if (mover != null) {
            mover.forwardDrive = Input.GetAxis("Vertical");
            mover.rotateDrive = Input.GetAxis("Horizontal");
        }
        if (weapon != null) {
    		weapon.actuate = (Input.GetKey(KeyCode.Alpha1)) ? 1f : 0f;
        }
    }
}
