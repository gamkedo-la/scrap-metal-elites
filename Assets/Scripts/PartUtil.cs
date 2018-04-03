using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PartUtil {

    const string hideTagKey = "part.hide";
    const string dontsaveTagKey = "part.dontsave";
    const string readonlyTagKey = "part.readonly";
    public static ConfigTag hideTag;
    public static ConfigTag dontsaveTag;
    public static ConfigTag readonlyTag;

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
        guids = AssetDatabase.FindAssets("t:ConfigTag " + readonlyTagKey);
        if (guids.Length > 0) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            readonlyTag = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConfigTag)) as ConfigTag;
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
            go.transform.parent = root.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
        }

        // look in config to determine if we are in display or build mode
        if (hideTag != null && config != null && config.Get<bool>(hideTag)) {
            go.hideFlags |= HideFlags.HideInHierarchy;
        }
        if (dontsaveTag != null && config != null && config.Get<bool>(dontsaveTag)) {
            go.hideFlags |= HideFlags.DontSave;
        }
        if (readonlyTag != null && config != null && config.Get<bool>(readonlyTag)) {
            go.hideFlags |= HideFlags.NotEditable;
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

    public static void ApplyRigidBodyProperties(
        GameObject rigidbodyGo,
        FloatVariable mass,
        FloatVariable drag,
        FloatVariable angularDrag
    ) {
        if (rigidbodyGo == null) return;
        var rigidbody = rigidbodyGo.GetComponent<Rigidbody>();
        if (rigidbody == null) return;
        if (mass != null) {
            rigidbody.mass = mass.Value;
        }
        if (drag != null) {
            rigidbody.drag = drag.Value;
        }
        if (angularDrag != null) {
            rigidbody.angularDrag = angularDrag.Value;
        }
    }

    // FIXME: come back to this
    // want to hook Joiner mechanism into filling out a connected list in the part... need to research if non-serialized field can be used in scriptable object w/out data being saved
    public static T[] GetComponentsInChildren<T>(GameObject rootPartGo) {
        var allComponents = new List<T>();
        // find component of specified type in root's tree
        T[] components = rootPartGo.GetComponentsInChildren<T>();
        allComponents.AddRange(components);

        // find any part siblings components in root
        var siblings = rootPartGo.GetComponentsInChildren<Sibling>();
        for (var i=0; i<siblings.Length; i++) {
            // if part has external parts reference
            if (siblings[i].siblingGo != null) {
                allComponents.AddRange(PartUtil.GetComponentsInChildren<T>(siblings[i].siblingGo));
            }
        }
        return allComponents.ToArray();
    }

    public static void DestroyPartGo(GameObject partGo) {
        // find any sibling relationship w/ object
        var siblings = GetComponentsInChildren<Sibling>(partGo);
        for (var i=0; i<siblings.Length; i++) {
            DestroyPartGo(siblings[i].siblingGo);
            siblings[i].siblingGo = null;
        }
        // destroy top-level gameobject tree
        if (Application.isPlaying) {
            UnityEngine.Object.Destroy(partGo);
        } else {
            UnityEngine.Object.DestroyImmediate(partGo);
        }

    }

}
