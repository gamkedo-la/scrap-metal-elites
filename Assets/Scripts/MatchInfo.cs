using UnityEngine;

[System.Serializable]
public class MatchInfo {
    public string id;
    public NamedPrefab playerPrefab;
    public string enemyTitle;
    public NamedPrefab[] enemyPrefabs;
    // FIXME: todo
    // player/enemy spawn points
    // # of enemies at a time
    // list of hazards
    // hazard spawn points
    // arena choice
}
