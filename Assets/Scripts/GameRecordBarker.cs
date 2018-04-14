using UnityEngine;

public class GameRecordBarker: MonoBehaviour {
    public void OnGameRecord(GameRecord record) {
        Debug.Log(record.ToString());
    }
}
