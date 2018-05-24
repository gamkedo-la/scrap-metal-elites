using UnityEngine;
using UnityEngine.UI;

public class UxCreditsPanel: UxPanel {
    [Header("UI Reference")]
    public Button backButton;

    void Start() {
        // setup callbacks
        backButton.onClick.AddListener(OnBackClick);
    }

    public void OnBackClick() {
        Destroy(gameObject);
    }

}
