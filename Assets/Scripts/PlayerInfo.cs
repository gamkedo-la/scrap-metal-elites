using System;
using UnityEngine;

[System.Serializable]
public class MatchScoreInfo {
    public string matchID;
    public int score;
    public int time;
    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public void Update(MatchScoreInfo other) {
        if (other.score > score) {
            score = other.score;
        }
        if (other.time < time) {
            time = other.time;
        }
    }
}

[System.Serializable]
public class PlayerInfo {
    public string name;
    public int wins;
    public int losses;
    public MatchScoreInfo[] scoreboard;

    public static PlayerInfo FromJson(
        string jsonString
    ) {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public void AddWin(MatchScoreInfo scoreInfo) {
        wins += 1;
        // determine if scoreboard already has info for given matchID
        bool found = false;
        for (var i=0; i<scoreboard.Length; i++) {
            if (scoreboard[i].matchID == scoreInfo.matchID) {
                // update existing scoreboard w/ new match info
                scoreboard[i].Update(scoreInfo);
                found = true;
            }
        }
        // add new entry if not found
        if (!found) {
            Array.Resize(ref scoreboard, scoreboard.Length+1);
            scoreboard[scoreboard.Length-1] = scoreInfo;
        }
        Save();
    }

    public MatchScoreInfo GetScoreboard(string matchID) {
        for (var i=0; i<scoreboard.Length; i++) {
            if (scoreboard[i].matchID == matchID) {
                return scoreboard[i];
            }
        }
        return null;
    }

    public void AddLoss() {
        losses += 1;
        Save();
    }

    public void Save() {
        JsonStore.Save(JsonStore.SaveTag.Player, name, ToJson());
    }
}
