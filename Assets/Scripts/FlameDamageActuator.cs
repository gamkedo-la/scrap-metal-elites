using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamageActuator : MonoBehaviour {

    public float fireRate = 0.6f;
    public ParticleSystem plume;
    public ParticleSystem sparks;
    public Material burntMaterial;
    public int burnThreshold = 50;
    public int explodeThreshold = 10;
    public float explodeForce = 1000f;
    public bool debug;

    private Rigidbody rb;
    private Health health;
    private Material unburntMaterial;
    private Transform emitlocation;
    private bool is_burnt;
    private bool is_exploded;

    void Start () {
        // link health to our on percent change handler
        health = GetComponent<Health>();
        if (health != null) {
            health.onChangePercent.AddListener(OnHealthPercentChange);
        }
        unburntMaterial = GetComponent<Renderer>().material;
        rb = GetComponent<Rigidbody>();
        emitlocation = gameObject.transform;
	}

    private void OnParticleCollision(GameObject other)
    {

        //List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        //int numCollisionEvents = plume.GetCollisionEvents(other, collisionEvents);
        var damage = fireRate;

        if (debug) {
            Debug.Log(gameObject.name + " OnParticleCollision\n" +
                                        " particle: " + other.name +
                                        " fireRate: " + fireRate +
                                        " damage: " + damage);
        }

        if (health != null) {
            health.TakeDamage(Mathf.RoundToInt(damage), other);
        }

    }

    void Burn() {
        if (debug) {
            Debug.Log(gameObject.name + " burning");
        }
        is_burnt = true;
        if (unburntMaterial != null && burntMaterial != null) {
            unburntMaterial.CopyPropertiesFromMaterial(burntMaterial);
        }
    }

    void Explode() {
        if (debug) {
            Debug.Log(gameObject.name + " exploding");
        }
        is_exploded = true;
        if (rb != null) {
            rb.AddForce(transform.up * explodeForce);
        }
    }

    void OnHealthPercentChange(int newValue) {
        if (debug) {
            Debug.Log("FlameDamage OnHealthPercentChange: " + newValue);
        }
        // burnt threshold
        if (!is_burnt && newValue<burnThreshold) {
            Burn();
        }
        if (!is_exploded && newValue<explodeThreshold) {
            Explode();
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
