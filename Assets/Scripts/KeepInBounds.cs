using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class KeepInBounds : MonoBehaviour {
    public float maxDelta = 200f;  // thousand meters
    void Update() {
        if (transform.position.magnitude > maxDelta) {
            Destroy(gameObject);
        }
    }

}
