using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchScoreRow {
    public string playerName;
    public int score;
    public int time;

    public MatchScoreRow(
        string playerName,
        int score,
        int time
    ) {
        this.playerName = playerName;
        this.score = score;
        this.time = time;
    }

    public override string ToString() {
        return String.Format("{0} {1} {2}", playerName, score, time);
    }
}

[System.Serializable]
public class MatchScoreboard {
    public string matchID;
    public MatchScoreRow[] scoreboard;
    public int maxScores = 10;

    static Dictionary<string, string> matchTitleMap;

    static MatchScoreboard() {
        // create mapping between match IDs to titles to present
        matchTitleMap = new Dictionary<string, string>();
        matchTitleMap["deathmatch"] = "Death Match";
        matchTitleMap["champ1"] = "Challenger Match #1";
        matchTitleMap["champ2"] = "Challenger Match #2";
        matchTitleMap["champ3"] = "Challenger Match #3";
        matchTitleMap["champ4"] = "Challenger Match #4";
        matchTitleMap["champ5"] = "SME Championship";
    }

    public MatchScoreboard(
        string matchID
    ) {
        this.matchID = matchID;
    }

    public string GetTitle() {
        if (matchTitleMap.ContainsKey(matchID)) {
            return matchTitleMap[matchID];
        } else {
            return matchID;
        }
    }

    public static MatchScoreboard FromJson(
        string jsonString
    ) {
        if (jsonString == "") {
            return null;
        } else {
            return JsonUtility.FromJson<MatchScoreboard>(jsonString);
        }
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public void Save() {
        JsonStore.Save(JsonStore.SaveTag.Score, matchID, ToJson());
    }

    public void AddScore(
        string playerName,
        int score,
        int time
    ) {
        // null scoreboard
        if (scoreboard == null) {
            scoreboard = new MatchScoreRow[1];
            scoreboard[0] = new MatchScoreRow(playerName, score, time);
            Save();

        // see if score is better than other entries in scoreboard
        } else {
            var insertIndex = 0;
            // find position in scoreboard
            for (; insertIndex<scoreboard.Length; insertIndex++) {
                if (score > scoreboard[insertIndex].score) {
                    break;
                }
            }
            if (insertIndex < scoreboard.Length || scoreboard.Length < maxScores) {
                // add space in scoreboard (if not at max)
                if (scoreboard.Length < maxScores) {
                    Array.Resize(ref scoreboard, scoreboard.Length+1);
                    if (insertIndex < scoreboard.Length-2) {
                        scoreboard[scoreboard.Length-1] = scoreboard[scoreboard.Length-2];
                    }
                }
                // shift entries down
                for (var i=scoreboard.Length-1; i>insertIndex; i--) {
                    scoreboard[i] = scoreboard[i-1];
                }
                // insert
                scoreboard[insertIndex] = new MatchScoreRow(playerName, score, time);
                Save();
            }
        }
    }

    public MatchScoreRow[] GetScores() {
        return scoreboard;
    }

}
