using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UxPlayerSelect : UxPanel {
    [Header("UI Reference")]
    public InputField playerNameInput;
    public Dropdown playerDropdown;
    public Button playerCreateButton;
    public Button playerSelectButton;
    public Button backButton;

    [Header("Game Events")]
    public StringEvent playerSelected;
    public GameEvent selectCancelled;               // used to notify of cancel

    private string[] existingPlayers;

    void Start() {
        // start out w/ modal hidden
        Hide();
    }

    public void OnPlayerList(string message) {
        Debug.Log("UxPlayerSelect.OnPlayerList: " + message);
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
        playerCreateButton.onClick.AddListener(OnPlayerCreate);
        if (existingPlayers.Length > 0) {
            playerSelectButton.interactable = true;
            playerSelectButton.onClick.AddListener(OnPlayerSelect);
        } else {
            playerSelectButton.interactable = false;
        }
        backButton.onClick.AddListener(OnBackClick);
    }

    public void OnPlayerCreate() {
        if (playerNameInput != null) {
            // validation: check for empty name
            if (playerNameInput.text == "") return;
            // Hide panel
            Hide();
            // raise event for player selected
            var name = playerNameInput.text;
            if (playerSelected != null) {
                playerSelected.Raise(name);
            }
        }
    }

    public void OnPlayerSelect() {
        // Hide panel
        Hide();
        // raise event for player selected
        var name = existingPlayers[playerDropdown.value];
        if (playerSelected != null) {
            playerSelected.Raise(name);
        }
    }

    public void OnBackClick() {
        // Hide panel
        Hide();
        // raise event for cancel
        selectCancelled.Raise();
    }

}
