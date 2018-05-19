using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour {

    public FloatVariable masterVolume;
    public int maxChannels = 5;

    private float lastVolume;

    public void Update() {
        if (!Mathf.Approximately(lastVolume, masterVolume.Value)) {
            var factor = (lastVolume > 0) ? masterVolume.Value/lastVolume : 0;
            for (var i=0; i<channels.Count; i++) {
                if (factor > 0) {
                    channels[i].volume *= factor;
                } else {
                    channels[i].volume = masterVolume.Value;
                }
            }
        }
    }

    private List<AudioSource> channels;

    /*
    public AudioSource GetChannel() {
    }
    */

}
