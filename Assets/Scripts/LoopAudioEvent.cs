using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio/Loop")]
public class LoopAudioEvent : AudioEvent
{
	public AudioClip clip;
	public RangedFloat volumeRange;
	[MinMaxRange(0, 2)]
	public RangedFloat pitchRange;

	public override void Play(AudioEmitter emitter) {
		if (clip == null || emitter == null) return;
		var volume = Random.Range(volumeRange.minValue, volumeRange.maxValue);
		var pitch = Random.Range(pitchRange.minValue, pitchRange.maxValue);
		emitter.Loop(clip, volume, pitch);
	}
}
