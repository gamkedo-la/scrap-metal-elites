using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlamethrower : MonoBehaviour {

    // redo as private to get from child objects?
    // do sound playing check as bool?
    public ParticleSystem Fire;
    public ParticleSystem Smoke;
    public ParticleSystem Plume;
    public AudioSource Roar;
    	
	void Update () {

        if (Input.GetButton("Fire1"))
        {
            Fire.Emit(1);
            Smoke.Emit(1);
            Plume.Emit(2);
            if (Roar.isPlaying) {
                return;
            }
            else {
                Roar.Play();
            }
        }

        if (Input.GetButtonUp("Fire1")) {
             
            Roar.Stop();
        }

	}
}
