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

    public override string ToString() {
        return String.Format("Best Score: {0} - Best Time {1}", score, Match.FmtTimerMsg(true, time));
    }
}
