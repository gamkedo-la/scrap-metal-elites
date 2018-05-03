using UnityEngine;

public enum GameMode {
    DeathMatch,
    Championship
}

[CreateAssetMenu(fileName = "gameInfo", menuName = "Variable/GameInfo")]
public class GameInfo : ScriptableObject {
    public GameMode gameMode;
}
