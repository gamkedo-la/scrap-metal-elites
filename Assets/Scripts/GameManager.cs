using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum GameMode {
        DeathMatch,
        Championship
    }

    [Header("Events")]
    public StringEvent wantPlayerSelect;            // used to notify player select modal to ask for player
    public StringEvent playerSelected;              // used to receive selected player
    public PlayerInfoEvent wantMatchSelect;         // used to notify match select modal to ask for match info
    public MatchInfoEvent matchSelected;            // used to receive selected match

    [Header("Game Config")]
    public bool debug = false;
    public GameMode gameMode = GameMode.DeathMatch;
    public Match match;

    private bool started = false;
    private PlayerInfo playerInfo;          // player name and stats
    private MatchInfo matchInfo = null;

    IEnumerator StatePlayerSelect() {
        if (debug) Debug.Log("StatePlayerSelect");
        string selectedPlayer = "";
        // create string event listener to listen for callback event from player select modal
        var listener = gameObject.AddComponent<StringEventListener>();
        listener.SetEvent(playerSelected);
        listener.Response.AddListener((msg)=>{selectedPlayer = msg;});

        // get list of player save files, then serialize to a single string
        var savedPlayerNames = JsonStore.ListByTag(JsonStore.SaveTag.Player);
        string serializedPlayerNames = "";
        if (savedPlayerNames.Length > 0) {
            serializedPlayerNames = string.Join(":", savedPlayerNames);
        }

        // trigger event to ask for player to be selected
        wantPlayerSelect.Raise(serializedPlayerNames);

        // wait for player to be selected
        yield return new WaitUntil(()=> selectedPlayer != "");

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

        // clean up, remove listener
        Destroy(listener);

        // transition to match select
        if (gameMode == GameMode.DeathMatch) {
            StartCoroutine(StateBotSelect());
        } else {
            StartCoroutine(StateMatchSelect());
        }
    }

    IEnumerator StateBotSelect() {
        if (debug) Debug.Log("StateBotSelect");
        // create match info event listener to listen for callback event from bot select modal
        var listener = gameObject.AddComponent<MatchInfoEventListener>();
        listener.SetEvent(matchSelected);
        listener.Response.AddListener((msg)=>{matchInfo = msg;});

        // trigger event to ask for match to be selected
        wantMatchSelect.Raise(playerInfo);

        // wait for match info to be selected
        yield return new WaitUntil(()=> matchInfo != null);

        // clean up, remove listener
        Destroy(listener);

        // transition to match play
        StartCoroutine(StateMatchPlay());
    }

    IEnumerator StateMatchSelect() {
        if (debug) Debug.Log("StateMatchSelect");
        yield return null;
    }

    IEnumerator StateMatchPlay() {
        if (debug) Debug.Log("StateMatchPlay");

        // Start the match
        match.PlayMatch(playerInfo, matchInfo);
        // FIXME: add event for match complete
        yield return null;
    }

    public void Update() {
        if (!started) {
            started = true;
            StartCoroutine(StatePlayerSelect());
        }
    }
}
