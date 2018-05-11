using UnityEngine;

[System.Serializable]
public class MatchInfo {
    public string id;
    public NamedPrefab playerPrefab;
    public string enemyTitle;
    public NamedPrefab[] enemyPrefabs;
    public NamedPrefab[] hazardPrefabs;
}
