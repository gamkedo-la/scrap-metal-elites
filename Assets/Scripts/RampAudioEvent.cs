using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio/Ramp")]
public class RampAudioEvent : AudioEvent {

	public AudioClip fadeInClip;
	public AudioClip loopClip;
	public AudioClip fadeOutClip;

	[MinMaxRange(0, 2)]
	public RangedFloat pitch;

	public RangedFloat volume;

	public override void Play(AudioSource audio) {
		var coRunner = audio.gameObject.GetComponent<CoroutineRunner>();
		audio.volume = Random.Range(volume.minValue, volume.maxValue);
		audio.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		if (coRunner != null) {
			if (Application.isPlaying) {
				Destroy(coRunner);
			} else {
				DestroyImmediate(coRunner);
			}
		}
		coRunner = audio.gameObject.AddComponent<CoroutineRunner>();
		coRunner.Run(PlayStartLoop(audio));
	}

	public override void Stop(AudioSource audio) {
		var coRunner = audio.gameObject.GetComponent<CoroutineRunner>();
		if (coRunner != null) {
			if (Application.isPlaying) {
				Destroy(coRunner);
			} else {
				DestroyImmediate(coRunner);
			}
		}
		coRunner = audio.gameObject.AddComponent<CoroutineRunner>();
		coRunner.Run(PlayStop(audio));
	}

     IEnumerator PlayStartLoop(AudioSource audio) {
		 if (fadeInClip != null) {
	         audio.clip = fadeInClip;
			 audio.loop = false;
	         audio.Play();
	         yield return new WaitForSeconds(audio.clip.length);
		 }
		 audio.loop = true;
         audio.clip = loopClip;
         audio.Play();
     }

     IEnumerator PlayStop(AudioSource audio) {
		 audio.loop = false;
		 if (fadeOutClip != null) {
	         audio.clip = fadeOutClip;
	         audio.Play();
		 } else {
			 audio.Stop();
		 }
         yield return new WaitForSeconds(audio.clip.length);
	 }

	/*
	enum RampState {
		Off,
		FadeIn,
		FadeOut,
		Loop
	}


	public RangedFloat volume;

	[MinMaxRange(0, 2)]
	public RangedFloat pitch;

	RampState state = RampState.Off;

	public override void Play(GameObject originator) {
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.Play();
		MonoBehaviour.StartCoroutine(DelayLoop());
	}

	IEnumerator DelayLoop() {
		yield return null;
	}


	public override void Stop(AudioSource source) {
	}
	*/
}
