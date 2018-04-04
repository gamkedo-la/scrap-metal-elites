using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "massVariable", menuName = "Variable/Mass")]
public class MassApplicator : ComponentApplicator {
    public float mass;
    public float drag;
    public float angularDrag;

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add rigid body properties
        var rigidbody = rigidbodyGo.GetComponent<Rigidbody>();
        if (rigidbody == null) return;
        rigidbody.mass = mass;
        rigidbody.drag = drag;
        rigidbody.angularDrag = angularDrag;
    }
}
