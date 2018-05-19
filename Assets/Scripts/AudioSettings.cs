using UnityEngine;

[CreateAssetMenu(menuName="Audio/Settings")]
public class AudioSettings : ScriptableObject {
	public bool muted = false;
	[Range (0f,1f)]
	public float sfxVolume;
	[Range (0f,1f)]
	public float musicVolume;
}
