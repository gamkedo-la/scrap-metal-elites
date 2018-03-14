using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaterialApplicator : MonoBehaviour {
    private Renderer[] renderers;

    public Material material {
        set {
            if (renderers != null) {
                for (var i=0; i<renderers.Length; i++) {
                    if (renderers[i] != null) renderers[i].material = value;
                }
            }
        }
    }

    void Start() {
        renderers = GetComponents<Renderer>();
    }
}
