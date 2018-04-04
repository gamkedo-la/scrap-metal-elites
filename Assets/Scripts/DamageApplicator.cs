using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "damageVariable", menuName = "Variable/Damage")]
public class DamageApplicator : ComponentApplicator {
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

        // apply damage actuat
        if (impactApplyDamage) {
            var actuator = rigidbodyGo.AddComponent<ImpactDamageActuator>();
            actuator.minDamage = impactMinDamage;
            actuator.maxDamage = impactMaxDamage;
            actuator.damageModifier = impactDamageModifier;
        }

        // FIXME: fire damage
    }
}
