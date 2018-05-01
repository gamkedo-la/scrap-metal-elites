using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UxPlayerSelect : MonoBehaviour {
    [Header("UI Reference")]
    public InputField playerNameInput;
    public Dropdown playerDropdown;
    public Button playerCreateButton;
    public Button playerSelectButton;
    public CanvasGroup canvasGroup;

    [Header("Game Events")]
    public StringEvent onPlayerSelect;

    private string[] existingPlayers;

    void Start() {
        // start out w/ modal hidden
        Hide();
    }

    void Display() {
        if (canvasGroup != null) {
            canvasGroup.alpha = 1f; //this makes everything transparent
            canvasGroup.blocksRaycasts = true; //this prevents the UI element to receive input events
            canvasGroup.interactable = true;
        }
    }

    void Hide() {
        if (canvasGroup != null) {
            canvasGroup.alpha = 0f; //this makes everything transparent
            canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
            canvasGroup.interactable = false;
        }
    }

    public void OnPlayerList(string message) {
        // display the modal
        Display();

        // parse message into player list
        if (message == "") {
            existingPlayers = new string[0];
        } else {
            existingPlayers = message.Split(':');
        }

        // handle player dropdown list
        if (playerDropdown != null) {
            playerDropdown.ClearOptions();
            if (existingPlayers.Length > 0) {
                playerDropdown.interactable = true;
                // set up existing player dropdown list
                for (var i=0; i<existingPlayers.Length; i++) {
                    var optionData = new Dropdown.OptionData();
                    optionData.text = existingPlayers[i];
                    playerDropdown.options.Add(optionData);
                }
                playerDropdown.RefreshShownValue();
            } else {
                playerDropdown.interactable = false;
            }
        }

        // setup button callbacks
        if (playerCreateButton != null) {
            playerCreateButton.onClick.AddListener(OnPlayerCreate);
        }
        if (playerSelectButton != null) {
            if (existingPlayers.Length > 0) {
                playerSelectButton.interactable = true;
                playerSelectButton.onClick.AddListener(OnPlayerSelect);
            } else {
                playerSelectButton.interactable = false;
            }
        }
    }

    public void OnPlayerCreate() {
        if (playerNameInput != null) {
            // validation: check for empty name
            if (playerNameInput.text == "") return;
            // Hide panel
            Hide();
            // raise event for player selected
            var name = playerNameInput.text;
            if (onPlayerSelect != null) {
                onPlayerSelect.Raise(name);
            }
        }
    }

    public void OnPlayerSelect() {
        // Hide panel
        Hide();
        // raise event for player selected
        var name = existingPlayers[playerDropdown.value];
        if (onPlayerSelect != null) {
            onPlayerSelect.Raise(name);
        }
    }
}
