using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlamethrower : MonoBehaviour, IActuator {

    public float actuate {
        get {
            return _actuate;
        }
        set {
            _actuate = value;
        }
    }

    // redo as private to get from child objects?
    // do sound playing check as bool?
    public ParticleSystem Fire;
    public ParticleSystem Smoke;
    public ParticleSystem Plume;
    public AudioSource Roar;
    private float _actuate = 0.0f;

	void Update () {

        if (actuate > 0f)
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
        } else {
            Roar.Stop();
        }

	}
}
