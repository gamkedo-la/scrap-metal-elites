using UnityEngine;
using UnityEngine.UI;

public class UxMainMenu : UxPanel {
    [Header("UI Reference")]
    public Button deathMatchButton;
    public Button titleMatchButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("State Variables")]
    public GameInfo gameInfo;

    [Header("Events")]
    public GameEvent modeSelected;            // used to notify main menu selection

    public void Start() {
        // start by displaying and setting up panel
        Display();
        OnModeSelection();
    }

    public void OnModeSelection() {
        // setup button callbacks
        deathMatchButton.onClick.AddListener(()=>{ OnGameModeClick(GameMode.DeathMatch); });
        titleMatchButton.onClick.AddListener(()=>{ OnGameModeClick(GameMode.Championship); });
        optionsButton.onClick.AddListener(OnOptionsClick);
        creditsButton.onClick.AddListener(OnCreditsClick);
        quitButton.onClick.AddListener(OnQuitClick);
    }

    public void OnGameModeClick(GameMode gameMode) {
        Debug.Log("OnGameModeClick");
        // game mode is stored in ScriptableObject for GameInfo, shared across scripts and scenes
        gameInfo.gameMode = gameMode;
        // raise notification that game mode has been selected
        modeSelected.Raise();
        // hide
        Hide();
    }

    public void OnQuitClick() {
        Application.Quit();
    }

    public void OnOptionsClick() {
    }

    public void OnCreditsClick() {
    }

}
