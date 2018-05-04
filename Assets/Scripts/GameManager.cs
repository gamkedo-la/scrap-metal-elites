using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("UI Reference")]
    public Canvas canvas;                           // the top-level canvas

    [Header("Prefabs")]
    public GameObject mainMenuPrefab;
    public GameObject playerSelectPanelPrefab;
    public GameObject deathMatchBotSelectPanelPrefab;
    public GameObject titleMatchPanelPrefab;

    [Header("Events")]
    public GameEvent cancelSelected;                // used to notify of cancel events (backing out of a panel)
    public GameEvent gameModeSelected;              // used to notify when game mode has been selected
    public StringEvent wantPlayerSelect;            // used to notify player select modal to ask for player
    public StringEvent playerSelected;              // used to receive selected player
    public PlayerInfoEvent wantMatchSelect;         // used to notify match select modal to ask for match info
    public MatchInfoEvent matchSelected;            // used to receive selected match
    public GameEvent arenaPrepared;                 // used to notify when arena has been prepared
    public GameEvent wantMatchStart;                // used to notify when match should be started
    public GameEvent matchFinished;                 // used to notify when match has been completed

    [Header("State Variables")]
    public GameInfo gameInfo;

    [Header("Game Config")]
    public bool debug = false;

    private bool startStateEngine = false;
    private PlayerInfo playerInfo;          // player name and stats
    private MatchInfo matchInfo = null;
    private string mainScene = "newMainMenu";
    private string arenaScene = "Arena DeathMatch";

    private static bool managerInstantiated = false;

    void Awake() {
        // game manager is not destroyed across scene loads, but may be re-instantiated during scene load
        // if that happens, make sure that only one manager runs the state engine
        if (!managerInstantiated) {
            managerInstantiated = true;
            startStateEngine = true;
            // the current instance is the controlling instance, make sure it doesn't get destroyed on scene load
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Update() {
        if (startStateEngine) {
            startStateEngine = false;
            StartCoroutine(StateMainMenu());
        }
    }

    IEnumerator StateMainMenu() {
        if (debug) Debug.Log("StateMainMenu");
        // instantiate main menu prefab (under canvas)
        var panelGo = Instantiate(mainMenuPrefab, canvas.gameObject.transform);
        yield return null;      // wait a frame for panel initialization

        // create listener for gameModeSelected event
        var selected = false;
        var listener = panelGo.AddComponent<GameEventListener>();
        listener.SetEvent(gameModeSelected);
        listener.Response.AddListener(()=>{selected = true;});

        // wait for gameModeSelected event
        yield return new WaitUntil(() => selected);

        Debug.Log("GameMode: " + gameInfo.gameMode);

        // clean up
        Destroy(panelGo);

        // transition StatePlayerSelect
        StartCoroutine(StatePlayerSelect());
    }

    IEnumerator StatePlayerSelect() {
        if (debug) Debug.Log("StatePlayerSelect");

        // load panel
        var panelGo = Instantiate(playerSelectPanelPrefab, canvas.gameObject.transform);
        yield return null;      // wait a frame for panel initialization
        string selectedPlayer = "";
        bool cancelled = false;

        // create string event listener to listen for callback event from player select modal
        var listener = panelGo.AddComponent<StringEventListener>();
        listener.SetEvent(playerSelected);
        listener.Response.AddListener((msg)=>{selectedPlayer = msg;});
        var geListener = panelGo.AddComponent<GameEventListener>();
        geListener.SetEvent(cancelSelected);
        geListener.Response.AddListener(()=>{cancelled = true;});

        // get list of player save files, then serialize to a single string
        var savedPlayerNames = JsonStore.ListByTag(JsonStore.SaveTag.Player);
        string serializedPlayerNames = "";
        if (savedPlayerNames.Length > 0) {
            serializedPlayerNames = string.Join(":", savedPlayerNames);
        }

        // trigger event to ask for player to be selected
        wantPlayerSelect.Raise(serializedPlayerNames);

        // wait for player to be selected
        yield return new WaitUntil(()=> selectedPlayer != "" || cancelled);

        // clean up
        Destroy(panelGo);

        // go back to main menu if cancelled
        if (cancelled) {
            StartCoroutine(StateMainMenu());

        // otherwise, advance, based on game state
        } else {
            // if player is selected from existing saved players... load that player
            if (System.Array.Exists(savedPlayerNames, element => element == selectedPlayer)) {
                var json = JsonStore.Load(JsonStore.SaveTag.Player, selectedPlayer);
                playerInfo = PlayerInfo.FromJson(json);

            // otherwise create and save new player record
            } else {
                playerInfo = new PlayerInfo();
                playerInfo.name = selectedPlayer;
                JsonStore.Save(JsonStore.SaveTag.Player, selectedPlayer, playerInfo.ToJson());
            }

            // transition to match select
            StartCoroutine(StateMatchSelect());
        }
    }

    // Death match bot selection
    IEnumerator StateMatchSelect() {
        if (debug) Debug.Log("StateMatchSelect");

        // load panel
        GameObject panelGo;
        if (gameInfo.gameMode == GameMode.DeathMatch) {
            panelGo = Instantiate(deathMatchBotSelectPanelPrefab, canvas.gameObject.transform);
        } else {
            panelGo = Instantiate(titleMatchPanelPrefab, canvas.gameObject.transform);
        }
        yield return null;      // wait a frame for panel initialization
        bool cancelled = false;

        // create match info event listener to listen for callback event from bot select modal
        var listener = panelGo.AddComponent<MatchInfoEventListener>();
        listener.SetEvent(matchSelected);
        listener.Response.AddListener((msg)=>{matchInfo = msg;});
        var geListener = panelGo.AddComponent<GameEventListener>();
        geListener.SetEvent(cancelSelected);
        geListener.Response.AddListener(()=>{cancelled = true;});

        // trigger event to ask for match to be selected
        wantMatchSelect.Raise(playerInfo);

        // wait for match info to be selected
        yield return new WaitUntil(()=> matchInfo != null || cancelled);

        // clean up, remove listener
        Destroy(panelGo);

        // transition back to player select if cancelled
        if (cancelled) {
            StartCoroutine(StatePlayerSelect());
        } else {
            // transition to match play
            StartCoroutine(StateMatchPlay());
        }
    }

    IEnumerator StateMatchPlay() {
        if (debug) Debug.Log("StateMatchPlay");

        // store current player/match info into shared GameInfo
        gameInfo.playerInfo = playerInfo;
        gameInfo.matchInfo = matchInfo;

        // load the arena
        SceneManager.LoadScene(arenaScene);
        yield return null;

        // wait for arena to signal it is ready
        /*
        var arenaReady = false;
        var listener = gameObject.AddComponent<GameEventListener>();
        listener.SetEvent(arenaPrepared);
        listener.Response.AddListener(()=>{arenaReady = true;});
        */

        // Start the match
        wantMatchStart.Raise();
        //match.PlayMatch(playerInfo, matchInfo);
        // FIXME: add event for match complete
        yield return null;
    }

}
