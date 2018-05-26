using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "damageVariable", menuName = "Variable/Damage")]
public class DamageApplicator : ComponentApplicator {
    // damage sources
    [Tooltip("Should this part react to impact damage")]
    public bool impactApplyDamage;
    [Tooltip("Minimum impact damage threshold")]
    public float impactMinDamage;
    [Tooltip("Max impact damage threshold")]
    public float impactMaxDamage;
    [Tooltip("Scale factor for incoming damage, lower value means less damage")]
    public float impactDamageModifier;
    [Tooltip("Emit screws on damage")]
    public bool impactScrews;
    [Tooltip("Amount of damage required to emit screws")]
    public float impactScrewDamage;

    public float smallImpactSfxThreshold;
    public AudioEvent smallImpactSfx;
    public float mediumImpactSfxThreshold;
    public AudioEvent mediumImpactSfx;
    public float largeImpactSfxThreshold;
    public AudioEvent largeImpactSfx;

    [Tooltip("Should this part react to fire damage")]
    public bool fireApplyDamage;
    [Tooltip("Damage to apply to health for each flame particle collision")]
    public float fireRate = 1f;
    [Tooltip("damage delay between flame particles")]
    [Range(0f,1f)]
    public float fireDamageDelay = 0f;
    [Tooltip("New material to add once burnt")]
    public Material burntMaterial;
    [Tooltip("Percent of health to fall under before applying burn")]
    public int burnThreshold = 50;
    [Tooltip("Percent of health to fall under before applying explode")]
    public int explodeThreshold = 10;
    [Tooltip("Amount of force to apply during explosion")]
    public float explodeForce = 1000;
    public bool debug;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // apply impact damage actuator
        if (impactApplyDamage) {
            var actuator = rigidbodyGo.AddComponent<ImpactDamageActuator>();
            actuator.minDamage = impactMinDamage;
            actuator.maxDamage = impactMaxDamage;
            actuator.damageModifier = impactDamageModifier;
            actuator.emitScrews = impactScrews;
            actuator.minScrewDamage = impactScrewDamage;
            actuator.smallImpactSfx = smallImpactSfx;
            actuator.smallImpactSfxThreshold = smallImpactSfxThreshold;
            actuator.mediumImpactSfx = mediumImpactSfx;
            actuator.mediumImpactSfxThreshold = mediumImpactSfxThreshold;
            actuator.largeImpactSfx = largeImpactSfx;
            actuator.largeImpactSfxThreshold = largeImpactSfxThreshold;
            actuator.debug = debug;
        }

        // apply fire damage applicator
        if (fireApplyDamage) {
            var actuator = rigidbodyGo.AddComponent<FlameDamageActuator>();
            actuator.fireRate = fireRate;
            actuator.fireDamageDelay = fireDamageDelay;
            //actuator.plume = plume;
            //actuator.sparks = sparks;
            actuator.burntMaterial = burntMaterial;
            actuator.burnThreshold = burnThreshold;
            actuator.explodeThreshold = explodeThreshold;
            actuator.debug = debug;
        }
    }
}
