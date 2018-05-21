using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AudioEmitter : MonoBehaviour {

    public AudioEvent audioEvent;
    public UnitFloatVariable masterVolume;
    public float volumeCheckInterval = .1f;

    public GameObjectEvent onDone;

    private float lastVolume;
    public bool playing = false;
    private AudioSource audioSource;

    private AudioClip clip;     // current clip that is playing
    private float clipVolume;   // assigned volume for clip
    private float clipPitch;    // assigned pitch for clip

    public void Awake() {
        onDone = new GameObjectEvent();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Setup(
        UnitFloatVariable masterVolume,
        AudioEvent audioEvent
    ) {
        this.masterVolume = masterVolume;
        this.audioEvent = audioEvent;
    }

    IEnumerator VolumeWatcher() {
        while (playing) {
            // check master volume against last volume
            if (!Mathf.Approximately(lastVolume, masterVolume.Value)) {
                audioSource.volume = clipVolume * masterVolume.Value;
                lastVolume = masterVolume.Value;
            }
            yield return new WaitForSecondsRealtime(volumeCheckInterval);
        }
    }

    IEnumerator PlayOnce() {
        // if already playing, bail
        if (Application.isPlaying && playing) yield break;

        // setup audio source
        audioSource.clip = clip;
        if (masterVolume != null) {
            audioSource.volume = clipVolume * masterVolume.Value;
        } else {
            audioSource.volume = clipVolume;
        }
        audioSource.pitch = clipPitch;

        // start playback
        playing = true;
        audioSource.loop = false;
        audioSource.Play();

        // start volume watcher
        if (masterVolume != null) {
            StartCoroutine(VolumeWatcher());
        }

        // wait for clip to be done (or stopped)
        var timer = clip.length;
        var startTime = Time.fixedTime;
        var currentDelta = Time.fixedTime - startTime;
        while (playing && (currentDelta < timer)) {
            yield return null;
            currentDelta = Time.fixedTime - startTime;
        }
        playing = false;
        audioSource.Stop();

        // raise onDone event
        onDone.Invoke(gameObject);
    }

    IEnumerator PlayLoop() {
        // if already playing, bail
        if (Application.isPlaying && playing) yield break;

        // setup audio source
        audioSource.clip = clip;
        if (masterVolume != null) {
            audioSource.volume = clipVolume * masterVolume.Value;
        } else {
            audioSource.volume = clipVolume;
        }
        audioSource.pitch = clipPitch;

        // start playback
        playing = true;
        audioSource.loop = true;
        audioSource.Play();

        // start volume watcher
        if (masterVolume != null) {
            StartCoroutine(VolumeWatcher());
        }

        // wait for clip to be stopped
        while (playing) {
            yield return null;
        }
        playing = false;
        audioSource.Stop();

        // raise onDone event
        onDone.Invoke(gameObject);
    }

    public void Play(AudioClip clip, float volume, float pitch) {
        this.clipVolume = volume;
        this.clipPitch = pitch;
        this.clip = clip;
        StartCoroutine(PlayOnce());
    }

    public void Loop(AudioClip clip, float volume, float pitch) {
        this.clipVolume = volume;
        this.clipPitch = pitch;
        this.clip = clip;
        StartCoroutine(PlayLoop());
    }

    public void Stop() {
        playing = false;
        if (audioSource != null) {
            audioSource.Stop();
        }
    }

}
