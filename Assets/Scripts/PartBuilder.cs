using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[ExecuteInEditMode]
public class PartBuilder : MonoBehaviour {
    public Part part;
    private GameObject partGo;

    void Build() {
        if (part != null) {
            var displayConfig = new PartConfig();
            // if just showing in editor, set readonly and dontsave flag on generated objects
            if (!Application.isPlaying) {
                displayConfig.Save<bool>(ConfigTag.PartReadOnly, true);
                displayConfig.Save<bool>(ConfigTag.PartDontSave, true);
            }
            partGo = part.Build(displayConfig, gameObject, part.name);
        }
    }

    void Clear() {
        if (partGo != null) {
            PartUtil.DestroyPartGo(partGo);
            partGo = null;
        }
    }

    public virtual void OnEnable() {
        // catch case where enable is called when transitioning into play mode
#if UNITY_EDITOR
        if (EditorApplication.isPlayingOrWillChangePlaymode && !Application.isPlaying) {
            return;
        }
#endif
        Build();
    }

    public virtual void OnDisable() {
        if (!Application.isPlaying) {
            Clear();
        }
    }

    void OnDestroy() {
        if (!Application.isPlaying) {
            Clear();
        }
    }

}
