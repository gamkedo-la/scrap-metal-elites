using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UxHealthBar : MonoBehaviour {
    [Header("UI Reference")]
    public RectTransform healthBarForeground;
    [Range(0,100)]
    public float fill = 100;

    void Update() {
        if (healthBarForeground != null) {
            healthBarForeground.localScale = new Vector3(fill/100f, 1f, 1f);
        }
    }

    public void OnHealthPercent(int value) {
        fill = value;
    }
}
