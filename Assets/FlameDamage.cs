using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamage : MonoBehaviour {

    //public GameObject target;
    private GameObject target;
    private float FireRate = 0.4f;
    public float FireDamage = 100f;
    public ParticleSystem plume;

    // Use this for initialization
    void Start () {

        target = GetComponent<GameObject>();
        
	}

    private void OnParticleCollision(GameObject plume)
    {

        FireDamage -= FireRate;
        Debug.Log("Health left:"+FireDamage);
    }


    // Update is called once per frame
    void Update () {
		
	}
}
