using UnityEngine;

public class TrySave : MonoBehaviour {
    void Awake() {
        var info = new PlayerInfo();
        info.name = "bob";
        info.wins = 3;
        info.losses = 1;
        JsonStore.Save(JsonStore.SaveTag.Player, "bob", info.ToJson());
        JsonStore.Save(JsonStore.SaveTag.Player, "sue", info.ToJson());
        JsonStore.ListByTag(JsonStore.SaveTag.Player);
    }
}
