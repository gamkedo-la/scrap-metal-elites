using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "health", menuName = "Variable/Health")]
public class HealthApplicator : ComponentApplicator {
    public float maxHealth;
    // damage sources
    public bool impactApplyDamage;
    public float impactMinDamage;
    public float impactMaxDamage;
    public float impactDamageModifier;
    public bool fireApplyDamage;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add fixed joint component to target
        var health = rigidbodyGo.AddComponent<Health>();
        health.maxHealth = (int) maxHealth;

        if (impactApplyDamage) {
            var applicator = rigidbodyGo.AddComponent<ImpactDamageApplicator>();
            applicator.minDamage = impactMinDamage;
            applicator.maxDamage = impactMaxDamage;
            //applicator.debug = true;
            applicator.damageModifier = impactDamageModifier;
        }
    }
}
