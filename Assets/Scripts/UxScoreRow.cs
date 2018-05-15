using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UxScoreRow : UxListElement {
    [Header("UI Reference")]
    public Text playerNameText;
    public Text timeText;
    public Text scoreText;

    public void Setup(
        string playerName,
        string time,
        string score
    ) {
        playerNameText.text = playerName.ToUpper();
        timeText.text = time;
        scoreText.text = score;
    }

}
