using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UxScoreboard : UxPanel {

    [Header("UI Reference")]
    public UxListController uxScoreList;
    public Text matchText;
    public Button prevButton;
    public Button nextButton;
    public Button backButton;

    [Header("Events")]
    public GameEvent backSelected;               // used to notify of cancel/back

    [Header("Prefabs")]
    public GameObject rowPrefab;

    [Header("Config")]
    public bool debug = false;

    private string[] matchIDs;
    private int matchIndex;
    private MatchScoreboard board;

    public void Start() {
        // load match info
        matchIDs = JsonStore.ListByTag(JsonStore.SaveTag.Score);
        matchIndex = 0;
        if (matchIDs.Length > 0) {
            var jsonStr = JsonStore.Load(JsonStore.SaveTag.Score, matchIDs[matchIndex]);
            board = MatchScoreboard.FromJson(jsonStr);
            ShowBoard();
        } else {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }

        // set back button callback
        backButton.onClick.AddListener(OnBack);
    }

    public void ShowBoard() {
        uxScoreList.Clear();
        matchText.text = board.GetTitle();
        // show rows in scoreboard
        var scores = board.GetScores();
        for (var i=0; i<scores.Length; i++) {
            // instantiate a new ux match row for each configured match
            var uxRow = Instantiate(rowPrefab);
            var uxRowCtrl = uxRow.GetComponent<UxScoreRow>();
            uxRowCtrl.Setup(scores[i].playerName, Match.FmtTimerMsg(true, scores[i].time), scores[i].score.ToString());
            uxScoreList.Add(uxRowCtrl);
        }
        for (var i=scores.Length; i<board.maxScores; i++) {
            // instantiate empty row
            var uxRow = Instantiate(rowPrefab);
            var uxRowCtrl = uxRow.GetComponent<UxScoreRow>();
            uxRowCtrl.Setup("", "", "");
            uxScoreList.Add(uxRowCtrl);
        }

        // hook up buttons - previous
        if (matchIndex > 0) {
            prevButton.gameObject.SetActive(true);
            prevButton.onClick.AddListener(OnPrevious);
        } else {
            prevButton.gameObject.SetActive(false);
        }
        // next
        if (matchIndex < matchIDs.Length-1) {
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(OnNext);
        } else {
            nextButton.gameObject.SetActive(false);
        }

    }

    public void OnPrevious() {
        if (matchIndex > 0) {
            matchIndex -= 1;
            var jsonStr = JsonStore.Load(JsonStore.SaveTag.Score, matchIDs[matchIndex]);
            board = MatchScoreboard.FromJson(jsonStr);
            ShowBoard();
        }
    }

    public void OnNext() {
        if (matchIndex < matchIDs.Length-1) {
            matchIndex += 1;
            var jsonStr = JsonStore.Load(JsonStore.SaveTag.Score, matchIDs[matchIndex]);
            board = MatchScoreboard.FromJson(jsonStr);
            ShowBoard();
        }
    }

    public void OnBack() {
        Hide();
        if (backSelected != null) {
            backSelected.Raise();
        }
    }
}
