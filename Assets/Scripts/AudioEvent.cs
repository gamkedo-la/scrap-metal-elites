using UnityEngine;

public abstract class AudioEvent : ScriptableObject {
	public AudioKind audioKind = AudioKind.Sfx;
	public abstract void Play(AudioEmitter emitter);
	public virtual void Stop(AudioEmitter emitter) {
		if (emitter != null) {
			emitter.Stop();
		}
	}
}
