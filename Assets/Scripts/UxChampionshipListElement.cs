using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UxChampionshipListElement : UxListElement {
    [Header("UI Reference")]
    public Text titleText;
    public Text scoreText;
    public Text playerNameText;
    public Text botNameText;
    public Button fightButton;

    public void Setup(
        string title,
        string score,
        string playerName,
        string botName,
        UnityAction onFightClick
    ) {
        titleText.text = title;
        scoreText.text = score;
        playerNameText.text = playerName.ToUpper();
        botNameText.text = botName.ToUpper();
        if (onFightClick != null) {
            fightButton.onClick.AddListener(onFightClick);
        } else {
            fightButton.interactable = false;
        }
    }

}
