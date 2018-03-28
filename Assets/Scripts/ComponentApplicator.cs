using UnityEngine;
using System.Collections;

public abstract class ComponentApplicator : ScriptableObject {
    public abstract void Apply(GameObject target);
}
