using UnityEngine;

public abstract class AudioEvent : ScriptableObject {
	public AudioSettings settings;
	public abstract void Play(AudioSource source);
	public abstract void Stop(AudioSource source);
}
