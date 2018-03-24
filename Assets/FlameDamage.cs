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
    private bool is_burnt;
    private bool is_exploded;
   
    
       
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
        if (FireDamage < showBurnt & is_burnt!=true) {

            is_burnt = true;
            unburnt.CopyPropertiesFromMaterial(burnt);
            
        }
        if (FireDamage < explodeBurnt & is_exploded!=true)
        {
            is_exploded = true;
            body.AddForce(transform.up * 1000);
            //sparks.transform.position = emitlocation.position;
            // sparks.transform.position = emitlocation;
            //sparks.Emit(20);
        }
    }

       
    void Update () {

        if (is_exploded)
        {
            sparks.transform.position = emitlocation.position;
            sparks.Emit(5);
        }
    }
}
