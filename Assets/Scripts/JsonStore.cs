using System.IO;
using UnityEngine;

public static class JsonStore {
    public enum SaveTag {
        Player,
        Score
    }

    public static void Save(SaveTag saveTag, string name, string json) {
        var filename = saveTag.ToString() + "_" + name + ".json";
        var path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("path: " + path);
        File.WriteAllText(path, json);
    }

    public static string Load(SaveTag saveTag, string name) {
        var filename = saveTag.ToString() + "_" + name + ".json";
        var path = Path.Combine(Application.persistentDataPath, filename);
        var readText = File.ReadAllText(path);
        return readText;
    }

}
