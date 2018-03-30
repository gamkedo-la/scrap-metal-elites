using UnityEngine;
using System.Collections;

public abstract class ComponentApplicator : ScriptableObject {
    public abstract void Apply(PartConfig config, GameObject target);
}
