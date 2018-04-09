using UnityEngine;
using System.Collections;

public enum HealthTag {
    Part,
    Module,
}

[CreateAssetMenu(fileName = "health", menuName = "Variable/Health")]
public class HealthApplicator : ComponentApplicator {
    public float maxHealth = 100;
    public HealthTag healthTag = HealthTag.Part;
    public bool debug = false;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add health component
        var health = rigidbodyGo.AddComponent<Health>();
        health.maxHealth = (int) maxHealth;
        health.healthTag = healthTag;
        health.debug = debug;
    }
}
