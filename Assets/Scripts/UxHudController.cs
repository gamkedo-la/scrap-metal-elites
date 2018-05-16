using UnityEngine;
using UnityEngine.UI;

public class UxHudController : UxPanel {
    [Header("UI Reference")]
    public UxListController uxEnemyList;
    public Text playerNameText;
    public UxHealthBar playerHealth;
    public Button menuButton;

    [Header("Events")]
    public GameRecordEvent eventChannel;

    [Header("Prefabs")]
    public GameObject enemyRowPrefab;

    void Start() {
        var listener = gameObject.AddComponent<GameRecordEventListener>();
        listener.SetEvent(eventChannel);
        listener.Response.AddListener(OnGameRecord);
    }

    void OnGameRecord(GameRecord record) {
        if (record.target != null) {
            if (record.tag == GameRecordTag.GamePlayerDeclared) {
                // player name
                playerNameText.text = record.target.name.ToUpper();
                // get bot health
                var health = record.target.GetComponent<BotHealth>();
                if (health != null) {
                    health.onChangePercent.AddListener(playerHealth.OnHealthPercent);
                }
            } else if (record.tag == GameRecordTag.GameEnemyDeclared) {
                var health = record.target.GetComponent<BotHealth>();
                if (health != null) {
                    // instantiate new enemy row
                    var uxRow = Instantiate(enemyRowPrefab);
                    var uxRowCtrl = uxRow.GetComponent<UxEnemyRow>();
                    if (uxRowCtrl != null) {
                        uxRowCtrl.Setup(record.target.name.ToUpper(), health.onChangePercent);
                    }
                    uxEnemyList.Add(uxRowCtrl);
                }
            }
        }
    }

}
