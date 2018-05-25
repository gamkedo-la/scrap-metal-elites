using System;
using UnityEngine;
using System.Collections;

public class ImpactDamageActuator : MonoBehaviour {
    public float damageModifier = 1f;
    // damage thresholds
    public float minDamage = 10f;
    public float maxDamage = 1000f;
    public bool debug = false;
    public float minScrewDamage = 30f;
    public bool emitScrews = false;
    public float smallImpactSfxThreshold;
    public AudioEvent smallImpactSfx;
    public float mediumImpactSfxThreshold;
    public AudioEvent mediumImpactSfx;
    public float largeImpactSfxThreshold;
    public AudioEvent largeImpactSfx;

    private Health health;
    private GameObject screwBurstPrefab;
    private GameObject rootGo;

    void Start() {
        rootGo = PartUtil.GetRootGo(gameObject);
        health = GetComponent<Health>();
        if (health == null) {
            health = PartUtil.GetComponentInParentBody<Health>(gameObject);
        }
        // enforce max damage > min damage
        if (maxDamage < minDamage) {
            maxDamage = minDamage;
        }
        if (emitScrews) {
            screwBurstPrefab = (GameObject)Resources.Load("ScrewBurst");
        }

    }

    void DebugCollision(Collision collision) {
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 3);
        }
    }

    void OnCollisionEnter(Collision collision) {
        // compute damage multiplier
        var multiplier = damageModifier;
        var multipliers = collision.gameObject.GetComponents<IImpactDamageModifier>();
        if (multipliers != null) {
            for (var i=0; i<multipliers.Length; i++) {
                multiplier *= multipliers[i].impactMultiplier;
            }
        }

        // compute damage
        var damage = collision.impulse.magnitude * multiplier;

        if (debug) {
            var dbgString = String.Format("{0} impact collision from {1}, impulse: {2}, multiplier: {3} damage {4}",
                gameObject.name, collision.gameObject.name, collision.impulse.magnitude, multiplier, damage);
            if (damage < minDamage) {
                dbgString += String.Format(" -- does not meet minimum {0}", minDamage);
            }
            Debug.Log(dbgString);
            DebugCollision(collision);
        }

        // apply damage thresholds
        if (damage < minDamage) {
            return;
        }
        if (damage > maxDamage) {
            damage = maxDamage;
        }

        // apply damage to component health
        if (health != null) {
            health.TakeDamage(Mathf.RoundToInt(damage), collision.gameObject);
        }

        // emit screws if damage was enough
        if (emitScrews && damage>minScrewDamage && screwBurstPrefab != null) {
            var position = collision.contacts[0].point;
            var rotation = Quaternion.LookRotation(collision.contacts[0].normal);

			GameObject.Instantiate(screwBurstPrefab, position, rotation);
        }

        // Play a sound if the colliding objects had a big impact.
        if (smallImpactSfx != null && damage>smallImpactSfxThreshold && damage<mediumImpactSfxThreshold) {
            smallImpactSfx.Play(AudioManager.GetInstance().GetEmitter(rootGo, smallImpactSfx));
        } else if (mediumImpactSfx != null && damage>mediumImpactSfxThreshold && damage<largeImpactSfxThreshold) {
            mediumImpactSfx.Play(AudioManager.GetInstance().GetEmitter(rootGo, mediumImpactSfx));
        } else if (largeImpactSfx != null && damage>largeImpactSfxThreshold) {
            largeImpactSfx.Play(AudioManager.GetInstance().GetEmitter(rootGo, largeImpactSfx));
        }

    }
}
