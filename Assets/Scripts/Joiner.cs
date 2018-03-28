using UnityEngine;

public class Joiner : MonoBehaviour {
    public Joint joint;
    public void Join(Rigidbody connectTo) {
        if (joint != null) {
            joint.connectedBody = connectTo;
        }
    }
}
