using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundOff: MonoBehaviour {
    public AudioEvent audioEvent;
    private AudioSource audio;
    public void Awake() {
        audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("collison enter");
        audioEvent.Play(audio);
    }
}
