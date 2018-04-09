using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour {
    private Renderer[] renderers;

    public Material material {
        set {
            if (renderers != null) {
                for (var i=0; i<renderers.Length; i++) {
                    renderers[i].material.CopyPropertiesFromMaterial(value);
                }
            }
        }
    }

    void Start() {
        renderers = GetComponents<Renderer>();
    }
}
