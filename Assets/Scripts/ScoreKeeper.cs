using UnityEngine;

public class ScoreKeeper : MonoBehaviour {
    [Header("Events")]
    public GameRecordEvent eventChannel;
    public StringEvent scoreBanner;

    [Header("Point Values")]
    //public int pointsSelfDamage = -5;
    public int pointsForDamage = 1;
    public int pointsForFlip = 250;
    public int pointsForJointBreak = 500;
    public int pointsForBotDeath = 2000;
    public bool debug = true;

    private int score = 0;
    private GameObject playerBotGo = null;
    private bool trackScore = false;

    void UpdateScore(int value) {
        if (debug) Debug.Log("update score: " + value + " new score: " + (score+value));
        score += value;
        if (scoreBanner != null) {
            scoreBanner.Raise(score.ToString());
        }
    }

    public int GetScore() {
        return score;
    }

    void Awake() {
        var listener = gameObject.AddComponent<GameRecordEventListener>();
        listener.SetEvent(eventChannel);
        listener.Response.AddListener(OnGameRecord);
    }

    public void OnGameRecord(GameRecord record) {
        switch (record.tag) {
        // start keeping track of score when player has been declared
        case GameRecordTag.GamePlayerDeclared:
            playerBotGo = record.target;
            if (debug) Debug.Log("tracking " + record.target.name + " as player");
            trackScore = true;
            break;
        case GameRecordTag.BotTookDamage:
            if (trackScore) {
                if (record.target == playerBotGo) {
                    UpdateScore(-record.intValue*pointsForDamage);
                } else {
                    UpdateScore(record.intValue*pointsForDamage);
                }
            }
            break;
        case GameRecordTag.BotJointBroke:
            if (trackScore) {
                if (record.target == playerBotGo) {
                    UpdateScore(-pointsForJointBreak);
                } else {
                    UpdateScore(pointsForJointBreak);
                }
            }
            break;
        case GameRecordTag.BotFlipped:
            if (trackScore) {
                if (record.target != playerBotGo) {
                    UpdateScore(pointsForFlip);
                }
            }
            break;
        case GameRecordTag.BotDied:
            if (trackScore) {
                if (record.target == playerBotGo) {
                    UpdateScore(-pointsForBotDeath);
                } else {
                    UpdateScore(pointsForBotDeath);
                }
            }
            break;
        }
    }
}
