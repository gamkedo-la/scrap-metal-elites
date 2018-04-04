using UnityEngine;
using System.Collections;

public class ImpactDamageActuator : MonoBehaviour {
    public float damageModifier = 1f;
    // damage thresholds
    public float minDamage = 10f;
    public float maxDamage = 1000f;
    public bool debug = false;

    //AudioSource audioSource;
    private Health health;

    void Start() {
        //audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        // enforce max damage > min damage
        if (maxDamage < minDamage) {
            maxDamage = minDamage;
        }
    }

    void DebugCollision(Collision collision) {
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 3);
        }
    }

    void OnCollisionEnter(Collision collision) {
        var force = collision.impulse.magnitude / Time.fixedDeltaTime;
        // compute damage (impulse * velocity * modifier)
        var damage = collision.impulse.magnitude * damageModifier;

        if (debug) {
            Debug.Log(gameObject.name + " OnCollisionEnter\n impulse: " + collision.impulse.magnitude +
                                        " velocity: " + collision.relativeVelocity.magnitude +
                                        " force: " + force +
                                        " times: " + (collision.impulse.magnitude * collision.relativeVelocity.magnitude) +
                                        " computed damage: " + damage);
            DebugCollision(collision);
        }

        // apply damage thresholds
        if (damage < minDamage) {
            if (debug) {
                Debug.Log("collision damage does not minimum");
            }
            return;
        }
        if (damage > maxDamage) {
            damage = maxDamage;
        }

        // apply damage to component health
        if (health != null) {
            health.TakeDamage(Mathf.RoundToInt(damage), collision.gameObject);
        }

        /*
        // Play a sound if the colliding objects had a big impact.
        if (collision.relativeVelocity.magnitude > 2) {
            audioSource.Play();
        }
        */
    }
}
