using UnityEngine;
using UnityEngine.UI;

public class UxDeathmatchPanelController : MonoBehaviour {
    public NamedPrefabRuntimeSet availableBots;

    [Header("UI Reference")]
    public InputField playerNameInput;
    public Dropdown playerBotDropdown;
    public InputField enemyNameInput;
    public Dropdown enemyBotDropdown;

    public void Start() {
        // set up bot lists
        if (availableBots != null) {
            // clear current dropdown options
            if (playerBotDropdown != null) {
                playerBotDropdown.ClearOptions();
            }
            if (enemyBotDropdown != null) {
                enemyBotDropdown.ClearOptions();
            }
            for (var i=0; i<availableBots.Items.Count; i++) {
                var optionData = new Dropdown.OptionData();
                optionData.text = availableBots.Items[i].name;

                // add optionData to dropdowns
                if (playerBotDropdown != null) playerBotDropdown.options.Add(optionData);
                if (enemyBotDropdown != null) enemyBotDropdown.options.Add(optionData);
            }
        }
    }
}
