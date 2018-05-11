using UnityEngine;
using UnityEngine.UI;

public class UxDeathmatchPanelController : MonoBehaviour {
    [Header("Events")]
    public MatchInfoEvent matchSelected;            // used to notify of selected match
    public GameEvent selectCancelled;               // used to notify of cancel

    [Header("UI Reference")]
    public InputField playerNameInput;
    public Dropdown playerBotDropdown;
    public RectTransform playerBotViewPanel;
    public InputField enemyNameInput;
    public Dropdown enemyBotDropdown;
    public RectTransform enemyBotViewPanel;
    public CanvasGroup canvasGroup;

    [Header("Prefabs")]
    public GameObject sandboxPrefab;

    [Header("Config")]
    public NamedPrefabRuntimeSet availableBots;

    private MatchInfo matchInfo;
    private UxSandboxController playerSandbox;
    private UxSandboxController enemySandbox;

    void Display() {
        canvasGroup.alpha = 1f; //this makes everything transparent
        canvasGroup.blocksRaycasts = true; //this prevents the UI element to receive input events
        canvasGroup.interactable = true;
    }

    void Hide() {
        canvasGroup.alpha = 0f; //this makes everything transparent
        canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        canvasGroup.interactable = false;
    }

    // callback triggered to start match/bot selection
    public void OnWantMatch(PlayerInfo playerInfo) {
        // awake display
        Display();

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
        playerNameInput.text = playerInfo.name;
        playerNameInput.interactable = false;
        enemyNameInput.text = availableBots.Items[0].name;
        enemyNameInput.interactable = false;

        // setup event handlers
        playerBotDropdown.onValueChanged.AddListener(delegate {OnDropdownChange(playerBotDropdown);});
        enemyBotDropdown.onValueChanged.AddListener(delegate {OnDropdownChange(enemyBotDropdown);});

        // initialize match info
        matchInfo = new MatchInfo();
        matchInfo.id = "deathmatch";
        matchInfo.playerPrefab = new NamedPrefab();
        matchInfo.playerPrefab.name = playerNameInput.text;
        matchInfo.playerPrefab.prefab = availableBots.Items[playerBotDropdown.value].prefab;
        matchInfo.enemyPrefabs = new NamedPrefab[1];
        matchInfo.enemyPrefabs[0] = new NamedPrefab();
        matchInfo.enemyPrefabs[0].name = enemyNameInput.text;
        matchInfo.enemyPrefabs[0].prefab = availableBots.Items[enemyBotDropdown.value].prefab;

        // initialize sandboxes
        var sandboxGo = Instantiate(sandboxPrefab, new Vector3(100,0,0), Quaternion.identity);
        playerSandbox = sandboxGo.GetComponent<UxSandboxController>();
        playerSandbox.externalPanel = playerBotViewPanel;
        playerSandbox.ShowBot(matchInfo.playerPrefab.prefab);
        sandboxGo = Instantiate(sandboxPrefab, new Vector3(-100,0,0), Quaternion.identity);
        enemySandbox = sandboxGo.GetComponent<UxSandboxController>();
        enemySandbox.externalPanel = enemyBotViewPanel;
        enemySandbox.ShowBot(matchInfo.enemyPrefabs[0].prefab);
    }

    public void OnDropdownChange(Dropdown dropdown) {
        var index = dropdown.value;
        if (dropdown == playerBotDropdown) {
            matchInfo.playerPrefab.prefab = availableBots.Items[dropdown.value].prefab;
            playerSandbox.ShowBot(matchInfo.playerPrefab.prefab);
        } else {
            enemyNameInput.text = availableBots.Items[index].name;
            matchInfo.enemyPrefabs[0].name = enemyNameInput.text;
            matchInfo.enemyPrefabs[0].prefab = availableBots.Items[dropdown.value].prefab;
            enemySandbox.ShowBot(matchInfo.enemyPrefabs[0].prefab);
        }
    }

    public void OnReady() {
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
        Hide();

        // send match select notification
        matchSelected.Raise(matchInfo);
    }

    public void OnCancel() {
        Hide();
        selectCancelled.Raise();
    }

    /*
    public void Start() {
        var json = "{\"name\":\"joe\",\"wins\":1,\"losses\":0,\"scoreboard\":[{\"matchID\":\"champ1\",\"score\":1400,\"time\":133}]}";
        OnWantMatch(PlayerInfo.FromJson(json));
    }
    */

}
