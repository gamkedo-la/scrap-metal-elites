using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamageActuator : MonoBehaviour {

    public float fireRate = 0.6f;
    //public ParticleSystem plume;
    public Material burntMaterial;
    public int burnThreshold = 50;
    public int explodeThreshold = 10;
    public float explodeForce = 1000f;
    public bool debug;

    private GameObject sparksPrefab;
    private ParticleSystem sparks;
    private Rigidbody rb;
    private Health health;
    private Material unburntMaterial;
    private MaterialActuator[] materialActuators;
    private bool is_burnt = false;
    private bool is_exploded = false;

    void Start () {
		sparksPrefab = (GameObject)Resources.Load("Sparks");
        // link health to our on percent change handler
        health = GetComponent<Health>();
        if (health != null) {
            health.onChangePercent.AddListener(OnHealthPercentChange);
        }
        materialActuators = PartUtil.GetComponentsInChildren<MaterialActuator>(gameObject);
        rb = GetComponent<Rigidbody>();
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
        if (materialActuators != null) {
            for (var i=0; i<materialActuators.Length; i++) {
                materialActuators[i].Assign(MaterialTag.Burnt);
            }
        }
    }

    void Explode() {
        if (debug) {
            Debug.Log(gameObject.name + " exploding");
        }
        is_exploded = true;
        // apply explosion force to rigidbody
        if (rb != null) {
            rb.AddForce(transform.up * explodeForce);
        }
        // instantiate sparks prefab
        if (sparksPrefab != null) {
			var sparksGo = GameObject.Instantiate(sparksPrefab, transform);
            if (sparksGo != null) {
                sparks = sparksGo.GetComponent<ParticleSystem>();
            }
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
        if (is_exploded && sparks != null)
        {
            sparks.Emit(1);
        }
    }
}
