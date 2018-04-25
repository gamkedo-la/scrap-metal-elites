using UnityEngine;

[System.Serializable]
public class PlayerInfo {
    public string name;
    public int wins;
    public int losses;

    public static PlayerInfo CreateFromJSON(
        string jsonString
    ) {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }

    public string SaveToString() {
        return JsonUtility.ToJson(this);
    }
}
