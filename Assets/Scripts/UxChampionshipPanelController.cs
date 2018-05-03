using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UxChampionshipPanelController : UxPanel {

    [Header("UI Reference")]
    public UxListController uxMatchList;
    public Button cancelButton;
    [Header("Events")]
    public MatchInfoEvent matchSelected;            // used to notify of selected match
    public GameEvent selectCancelled;               // used to notify of cancel
    [Header("Prefabs")]
    public GameObject matchRowPrefab;
    [Header("Config")]
    public bool debug = false;
    [Header("Match Config")]
    public MatchInfo[] matchConfig;

    private PlayerInfo playerInfo;
    private int selectedMatchIndex;
    private bool cancelled = false;

    public void OnWantMatchSelect(PlayerInfo playerInfo) {
        // enable panel display - clear any existing rows from match list
        Display();
        uxMatchList.Clear();

        // set player info
        this.playerInfo = playerInfo;

        // rebuild match list
        bool lastRoundWon = false;
        selectedMatchIndex = -1;
        for (var i=0; i<matchConfig.Length; i++) {
            string title, score, enemyTitle;
            UnityAction clickAction;
            var playerScore = playerInfo.GetScoreboard(matchConfig[i].id);
            if (i != matchConfig.Length-1) {
                title = System.String.Format("Challenger Match #{0}", i+1);
            } else {
                title = "SME Championship Round";
            }
            if (playerScore != null) {
                score = playerScore.ToString();
                enemyTitle = matchConfig[i].enemyTitle;
                {  // local scope to pass index to lambda
                    var localI = i;
                    clickAction = () => {selectedMatchIndex = localI;};
                }
            } else {
                if (lastRoundWon) {
                    score = "Do you have what it takes?";
                    enemyTitle = matchConfig[i].enemyTitle;
                    {  // local scope to pass index to lambda
                        var localI = i;
                        clickAction = () => {selectedMatchIndex = localI;};
                    }
                } else {
                    score = "Win previous round to advance!";
                    enemyTitle = "???";
                    clickAction = null;
                }
            }

            // instantiate a new ux match row for each configured match
            var uxRow = Instantiate(matchRowPrefab);
            var uxRowCtrl = uxRow.GetComponent<UxChampionshipListElement>();
            uxRowCtrl.Setup(title, score, playerInfo.name, enemyTitle, clickAction);
            uxMatchList.Add(uxRowCtrl);

            // setup cancel button callback
            cancelButton.onClick.AddListener(() => {cancelled = true;});

            // keep track of last round won
            lastRoundWon = playerScore != null;
        }

        // wait for match to be selected
        StartCoroutine(StateMatchSelect());
    }

    IEnumerator StateMatchSelect() {
        if (debug) Debug.Log("StatePlayerSelect");

        // wait for match to be selected
        yield return new WaitUntil(()=> selectedMatchIndex != -1 || cancelled);

        // if cancelled, raise cancelled event
        if (cancelled) {
            if (debug) Debug.Log("selection cancelled");
            if (selectCancelled != null) {
                selectCancelled.Raise();
            }

        // otherwise, raise match selected event
        } else {
            if (debug) Debug.Log("selected match: " + selectedMatchIndex + " id: ");
            matchConfig[selectedMatchIndex].playerPrefab.name = playerInfo.name;
            matchSelected.Raise(matchConfig[selectedMatchIndex]);
        }

        // clean up local handlers
        cancelButton.onClick.RemoveAllListeners();
        Hide();
    }

    public void Start() {
        var json = "{\"name\":\"joe\",\"wins\":1,\"losses\":0,\"scoreboard\":[{\"matchID\":\"champ1\",\"score\":1400,\"time\":133}]}";
        OnWantMatchSelect(PlayerInfo.FromJson(json));
    }
}
