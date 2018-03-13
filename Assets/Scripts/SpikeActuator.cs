using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeActuator : MonoBehaviour {

    void Update () {
    	if (Input.GetKeyDown(KeyCode.Alpha1)) {
    	    GetComponent<Animation>().Play("Armature|ArmatureAction");
    	}
    }
}
