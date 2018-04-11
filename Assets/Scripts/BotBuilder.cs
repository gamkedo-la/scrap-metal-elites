using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[ExecuteInEditMode]
public class BotBuilder : PartBuilder {
    public BotRuntimeSet botList;

    public override void OnEnable() {
        base.OnEnable();
        if (Application.isPlaying) {
            botList.Add(this);
        }
    }

    public override void OnDisable() {
        if (Application.isPlaying) {
            botList.Remove(this);
        }
        base.OnDisable();
    }

}
