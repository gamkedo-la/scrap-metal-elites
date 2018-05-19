using System.Collections;
using UnityEngine;

// class used to track audio event state for an object that should be emitting sound
[ExecuteInEditMode]
public class CoroutineRunner : MonoBehaviour {
    public void Run(IEnumerator stateFcn) {
        StartCoroutine(DestroyOnEnd(stateFcn));
    }

    IEnumerator DestroyOnEnd(IEnumerator stateFcn) {
        // wait for state fcn to end
        yield return stateFcn;

        // destroy self
        if (Application.isPlaying) {
            Destroy(this);
        } else {
            DestroyImmediate(this);
        }
    }

}
