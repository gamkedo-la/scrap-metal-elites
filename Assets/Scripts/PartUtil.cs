using System;
using UnityEngine;
using UnityEditor;

public static class PartUtil {

    const string hideTagKey = "part.hide";
    const string dontsaveTagKey = "part.dontsave";
    public static ConfigTag hideTag;
    public static ConfigTag dontsaveTag;

    // Static Initialization;
    static PartUtil() {
        var guids = AssetDatabase.FindAssets("t:ConfigTag " + hideTagKey);
        if (guids.Length > 0) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            hideTag = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConfigTag)) as ConfigTag;
        }
        guids = AssetDatabase.FindAssets("t:ConfigTag " + dontsaveTagKey);
        if (guids.Length > 0) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            dontsaveTag = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConfigTag)) as ConfigTag;
        }
    }

    public static GameObject BuildGo(
        PartConfig config,
        GameObject root,
        string label,
        params Type[] components
    ) {
        // create empty game object w/ rigidbody component
        var go = new GameObject(label, components);
        if (root != null) {
            // FIXME: make sure translation isn't happening here
            go.transform.parent = root.transform;
        }

        // look in config to determine if we are in display or build mode
        if (hideTag != null && config != null && config.Get<bool>(hideTag)) {
            go.hideFlags |= HideFlags.HideInHierarchy;
        }
        if (dontsaveTag != null && config != null && config.Get<bool>(dontsaveTag)) {
            go.hideFlags |= HideFlags.DontSave;
        }
        return go;
    }

    public static GameObject GetBodyGo(
        GameObject partGo
    ) {
        GameObject bodyGo = null;
        if (partGo != null) {
            // if partGo already has rigidbody component ...
            if (partGo.GetComponent<Rigidbody>() != null) {
                bodyGo = partGo;
            } else {
                var bodyTrans = partGo.transform.Find(partGo.name + ".body");
                if (bodyTrans != null && bodyTrans.gameObject.GetComponent<Rigidbody>() != null) {
                    bodyGo = bodyTrans.gameObject;
                }
            }
        }
        return bodyGo;
    }
}
