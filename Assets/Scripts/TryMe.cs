using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TryMe : MonoBehaviour {
    public AudioEvent audioEvent;

    void Update() {
        if ( Input.GetKeyDown(KeyCode.Alpha1)) {
            audioEvent.Play(GetComponent<AudioSource>());
        }
        if ( Input.GetKeyUp(KeyCode.Alpha1)) {
            audioEvent.Stop(GetComponent<AudioSource>());
        }
    }

}
