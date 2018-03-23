using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlamethrower : MonoBehaviour {

    public ParticleSystem Fire;
    public ParticleSystem Smoke;
    public ParticleSystem Plume;

    	
	void Update () {

        if (Input.GetButton("Fire1"))
        {
            Fire.Emit(1);
            Smoke.Emit(1);
            Plume.Emit(2);
        }

	}
}
