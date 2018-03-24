using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamage : MonoBehaviour {

   
    private float FireRate = 0.4f;
    public float FireDamage = 100f;
    private float showBurnt = 90f; 
    private float explodeBurnt = 70f; 
    public ParticleSystem plume;
    public ParticleSystem sparks;
    private Material unburnt;
    public Material burnt;
    private Rigidbody body;
    private Transform emitlocation;
    
   
    
       
    void Start () {       
        unburnt = GetComponent<Renderer>().material;
        body = GetComponent<Rigidbody>();

        emitlocation = gameObject.transform;

        
	}

    private void OnParticleCollision(GameObject plume)
    {
      

        if (FireDamage > 0) { 
        FireDamage -= FireRate;
        //Debug.Log("Health left:" + FireDamage);
        }
        if (FireDamage < showBurnt) {
           
            unburnt.CopyPropertiesFromMaterial(burnt);
            
        }
        if (FireDamage < explodeBurnt)
        {
            //body.AddForce(transform.up * 50);
            sparks.transform.position = emitlocation.position;
            // sparks.transform.position = emitlocation;
            sparks.Emit(20);
        }
    }

       
    void Update () {
		
	}
}
