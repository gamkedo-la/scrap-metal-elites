using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "damageModifier", menuName = "Variable/DamageModifier")]
public class DamageModifierApplicator : ComponentApplicator {
    [Range(0.01f, 50f)]
    public float impactDamageMultiplier = 1f;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add damage modifier
        var modifier = rigidbodyGo.AddComponent<ImpactDamageModifier>();
        modifier.impactDamageMultiplier = impactDamageMultiplier;
    }
}
