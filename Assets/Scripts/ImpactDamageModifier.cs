using UnityEngine;

public class ImpactDamageModifier : MonoBehaviour, IImpactDamageModifier {
    public float impactDamageMultiplier = 1f;

    public float impactMultiplier {
        get {
            return impactDamageMultiplier;
        }
    }
}
