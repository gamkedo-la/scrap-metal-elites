using UnityEngine;
using UnityEngine.UI;

public class UxDeathmatchPanelController : MonoBehaviour {
    public NamedPrefabRuntimeSet availableBots;

    [Header("UI Reference")]
    public InputField playerNameInput;
    public Dropdown playerBotDropdown;
    public InputField enemyNameInput;
    public Dropdown enemyBotDropdown;

    public UxSandboxController playerSandbox;
    public UxSandboxController enemySandbox;
    public Match match;

    private bool customPlayerName = false;
    private bool customEnemyName = false;
    private MatchInfo matchInfo;

    public void Start() {
        // input validation
        if (playerBotDropdown == null ||
            playerNameInput == null ||
            enemyBotDropdown == null ||
            enemyNameInput == null ||
            availableBots == null ||
            playerSandbox == null ||
            enemySandbox == null) {
            return;
        }

        // clear current dropdown options
        playerBotDropdown.ClearOptions();
        enemyBotDropdown.ClearOptions();

        // set up bot lists
        for (var i=0; i<availableBots.Items.Count; i++) {
            var optionData = new Dropdown.OptionData();
            optionData.text = availableBots.Items[i].name;

            // add optionData to dropdowns
            playerBotDropdown.options.Add(optionData);
            enemyBotDropdown.options.Add(optionData);
        }
        playerBotDropdown.RefreshShownValue();
        enemyBotDropdown.RefreshShownValue();

        // setup initial bot names based on current selection
        playerNameInput.text = availableBots.Items[0].name;
        enemyNameInput.text = availableBots.Items[0].name;

        // setup event handlers
        playerBotDropdown.onValueChanged.AddListener(delegate {OnDropdownChange(playerBotDropdown);});
        playerNameInput.onEndEdit.AddListener(delegate {OnNameChange(playerNameInput);});
        enemyBotDropdown.onValueChanged.AddListener(delegate {OnDropdownChange(enemyBotDropdown);});
        enemyNameInput.onEndEdit.AddListener(delegate {OnNameChange(enemyNameInput);});

        // initialize match info
        matchInfo = new MatchInfo();
        matchInfo.playerPrefab = new NamedPrefab();
        matchInfo.playerPrefab.name = playerNameInput.text;
        matchInfo.playerPrefab.prefab = availableBots.Items[playerBotDropdown.value].prefab;
        matchInfo.enemyPrefabs = new NamedPrefab[1];
        matchInfo.enemyPrefabs[0] = new NamedPrefab();
        matchInfo.enemyPrefabs[0].name = enemyNameInput.text;
        matchInfo.enemyPrefabs[0].prefab = availableBots.Items[enemyBotDropdown.value].prefab;

        // initialize sandboxes
        playerSandbox.ShowBot(matchInfo.playerPrefab.prefab);
        enemySandbox.ShowBot(matchInfo.enemyPrefabs[0].prefab);
    }

    public void OnDropdownChange(Dropdown dropdown) {
        var index = dropdown.value;
        if (dropdown == playerBotDropdown) {
            if (!customPlayerName) {
                playerNameInput.text = availableBots.Items[index].name;
                matchInfo.playerPrefab.name = playerNameInput.text;
            }
            matchInfo.playerPrefab.prefab = availableBots.Items[dropdown.value].prefab;
            playerSandbox.ShowBot(matchInfo.playerPrefab.prefab);
        } else {
            if (!customEnemyName) {
                enemyNameInput.text = availableBots.Items[index].name;
                matchInfo.enemyPrefabs[0].name = enemyNameInput.text;
            }
            matchInfo.enemyPrefabs[0].prefab = availableBots.Items[dropdown.value].prefab;
            enemySandbox.ShowBot(matchInfo.enemyPrefabs[0].prefab);
        }
    }

    public void OnNameChange(InputField input) {
        if (input == playerNameInput) {
            customPlayerName = true;
            matchInfo.playerPrefab.name = input.text;
        } else {
            customEnemyName = true;
            matchInfo.enemyPrefabs[0].name = input.text;
        }
    }

    public void OnReady() {
        if (match != null) {
            // disable sandboxes
            if (playerSandbox != null) {
                playerSandbox.Clear();
                playerSandbox.gameObject.SetActive(false);
            }
            if (enemySandbox != null) {
                enemySandbox.Clear();
                enemySandbox.gameObject.SetActive(false);
            }
            // disable the setup panel
            gameObject.SetActive(false);
            // start the match
            match.StartMatch(matchInfo);
        }
    }

}
