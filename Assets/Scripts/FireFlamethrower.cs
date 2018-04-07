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
    public Vector3 kickBackVector;
    public float kickBackForce;
    private float _actuate = 0.0f;
    private Rigidbody rb;
    private bool runDiscovery = true;

    void Discover() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = GetComponentInParent<Rigidbody>();
        }
    }

	void Update () {
        if (runDiscovery) {
            Discover();
            runDiscovery = false;
        }

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

            if (rb != null && kickBackForce > 0f && kickBackVector.magnitude > 0f) {
                var f = kickBackVector * kickBackForce;
                rb.AddRelativeForce(f);
            }
        } else {
            Roar.Stop();
        }
	}
}
