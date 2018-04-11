using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public SpawnPointRuntimeSet RuntimeSet;

    private void OnEnable() {
        RuntimeSet.Add(this);
    }

    private void OnDisable() {
        RuntimeSet.Remove(this);
    }
}
