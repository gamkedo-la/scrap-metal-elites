using UnityEngine;

public static class PartUtil {

    public static GameObject BuildEmptyBody(
        GameObject root,
        string label
    ) {
        // create empty game object w/ rigidbody component
        var bodyGo = new GameObject(label, typeof(Rigidbody));
        bodyGo.transform.parent = root.transform;
        return bodyGo;
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
