using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio/Loop")]
public class LoopAudioEvent : AudioEvent
{
	public AudioClip[] clips;
	public RangedFloat volumeRange;
	[MinMaxRange(0, 2)]
	public RangedFloat pitchRange;

	public override void Play(AudioEmitter emitter) {
		if (clips.Length == 0 || emitter == null) return;
		var clip = clips[Random.Range(0, clips.Length)];
		var volume = Random.Range(volumeRange.minValue, volumeRange.maxValue);
		var pitch = Random.Range(pitchRange.minValue, pitchRange.maxValue);
		emitter.Loop(clip, volume, pitch);
	}
}
