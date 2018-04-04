using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "part", menuName = "Parts/Part")]
public class Part : ScriptableObject {
    [Tooltip("Mass properties of the part model, not including connected parts, null means rigidbody defaults are used")]
    public MassApplicator mass;

    [Tooltip("Health associated with part, null means no health")]
    public HealthApplicator health;

    [Tooltip("How to apply damage to this part, null means no damage will be applied")]
    public DamageApplicator damage;

    [Tooltip("Models are the prefabs containing mesh and collider data for the main body of the part")]
    public ModelReference[] models;

    [Tooltip("Additional applicators for the part (e.g.: extra joints or other properties to apply to the part)")]
    public ComponentApplicator[] applicators;

    [Tooltip("References to other parts to be attached to this part")]
    public PartReference[] connectedParts;

    public virtual GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        if (label == null || label == "") {
            label = name;
        }

        // create empty parts container
        var partsGo = PartUtil.BuildGo(config, root, label);

        // create new rigid body for this part, set parts container as parent
        var rigidbodyGo = PartUtil.BuildGo(config, partsGo, label + ".body", typeof(Rigidbody));
        //PartUtil.ApplyRigidBodyProperties(rigidbodyGo, mass, drag, angularDrag);

        // apply part properties
        if (mass != null) {
            mass.Apply(config, partsGo);
        }
        if (health != null) {
            health.Apply(config, partsGo);
        }
        if (damage != null) {
            damage.Apply(config, partsGo);
        }

        // apply applicators to parts container
        if (applicators != null) {
            for (var i=0; i<applicators.Length; i++) {
                applicators[i].Apply(config, partsGo);
            }
        }

        // instantiate model under rigid body
        if (models != null) {
            for (var i=0; i<models.Length; i++) {
                models[i].Build(config, rigidbodyGo, label + ".model");
            }
        }

        // instantiate connected parts
        if (connectedParts != null && connectedParts.Length > 0) {
            for (var i=0; i<connectedParts.Length; i++) {
                var childGo = connectedParts[i].Build(config, partsGo, label + ".part");
                if (childGo != null) {
                    // join child part to current rigidbody
                    var joiner = childGo.GetComponent<Joiner>();
                    if (joiner != null) {
                        joiner.Join(rigidbodyGo.GetComponent<Rigidbody>());
                    }
                }
            }
        }
        return partsGo;
    }
}
