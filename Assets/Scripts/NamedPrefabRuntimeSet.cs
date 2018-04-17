using UnityEngine;

// class to associate a human readable name to a prefab
[System.Serializable]
public class NamedPrefab {
    public string name;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "namedPrefabs", menuName = "RuntimeSet/NamedPrefabs")]
public class NamedPrefabRuntimeSet : RuntimeSet<NamedPrefab>{}
