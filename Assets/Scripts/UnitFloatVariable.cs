using UnityEngine;

[CreateAssetMenu(menuName="Variable/UnitFloat")]
public class UnitFloatVariable : ScriptableObject {
    [Range(0,1)]
    public float Value;

    public void SetValue(float value) {
        Value = value;
    }

    public void SetValue(FloatVariable value) {
        Value = Mathf.Min(value.Value, 1f);
    }

    public void ApplyChange(float amount) {
        Value += amount;
        if (Value > 1f) {
            Value = 1f;
        }
    }

    public void ApplyChange(FloatVariable amount) {
        Value += amount.Value;
        if (Value > 1f) {
            Value = 1f;
        }
    }
}
