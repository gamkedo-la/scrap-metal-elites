using UnityEngine;
using UnityEngine.UI;

public class UxEnemyRow : UxListElement {
    [Header("UI Reference")]
    public Text nameText;
    public UxHealthBar healthBar;

    public void Setup(
        string name,
        OnHealthValueEvent onChangePercent
    ) {
        // set name
        nameText.text = name;
        // link healthbar to bot health event
        onChangePercent.AddListener(healthBar.OnHealthPercent);
    }

}
