using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum AudioKind {
    Sfx,
    Music,
    Voice,
}

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public static AudioManager GetInstance() {
        if (instance == null) {
            instance = new GameObject("AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
        }
        return instance;
    }

    public UnitFloatVariable sfxVolume;
    public UnitFloatVariable musicVolume;

    List<AudioEmitter> emitterPool;

    Dictionary<GameObject, Dictionary<AudioEvent, AudioEmitter>> activeEmitterMap;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        activeEmitterMap = new Dictionary<GameObject, Dictionary<AudioEvent, AudioEmitter>>();
        emitterPool = new List<AudioEmitter>();
    }

    Dictionary<AudioEvent, AudioEmitter> EventMapForActor(GameObject actor) {
        if (activeEmitterMap.ContainsKey(actor)) {
            return activeEmitterMap[actor];
        } else {
            var actorMap = new Dictionary<AudioEvent, AudioEmitter>();
            activeEmitterMap[actor] = actorMap;
            return actorMap;
        }
    }

    UnitFloatVariable VolumeForEvent(AudioEvent audioEvent) {
        if (audioEvent.audioKind == AudioKind.Sfx) {
            return sfxVolume;
        } else if (audioEvent.audioKind == AudioKind.Music) {
            return musicVolume;
        }
        return null;
    }

    AudioEmitter EmitterForEvent(GameObject actor, Dictionary<AudioEvent, AudioEmitter> eventMap, AudioEvent audioEvent) {
        if (eventMap.ContainsKey(audioEvent)) {
            return eventMap[audioEvent];
        } else {
            // allocate new emitter for event
            var emitter = EmitterFromPool();
            emitter.Setup(VolumeForEvent(audioEvent), audioEvent);
            // attach to actor, local position
            emitter.gameObject.transform.SetParent(actor.transform, false);
            // setup done event handler
            emitter.onDone.AddListener(OnEmitterDone);
            eventMap[audioEvent] = emitter;
            return emitter;
        }
    }

    AudioEmitter EmitterFromPool() {
        AudioEmitter emitter = null;
        if (emitterPool.Count > 0) {
            emitter = emitterPool[0];
            emitterPool.RemoveAt(0);
            return emitter;
        } else {
            // allocate new emitter
            emitter = new GameObject("audioEmitter", typeof(AudioEmitter)).GetComponent<AudioEmitter>();
            return emitter;
        }
    }

    void EmitterToPool(AudioEmitter emitter) {
        // reparent emitter to manager gameobject
        emitter.gameObject.transform.SetParent(gameObject.transform, false);
        // add back to pool
        emitterPool.Add(emitter);
    }

    public AudioEmitter GetEmitter(
        GameObject actor,
        AudioEvent audioEvent
    ) {
        // does the actor have an entry in the active emitter map
        var eventMap = EventMapForActor(actor);

        // allocate emitter
        return EmitterForEvent(actor, eventMap, audioEvent);
    }

    // execute this callback when emitter is done, so that it can be reclaimed and returned to pool
    public void OnEmitterDone(GameObject emitterGo) {
        var emitter = emitterGo.GetComponent<AudioEmitter>();
        if (emitter == null) return;

        // clear from emitter map
        if (emitterGo.transform.parent != null) {
            var actor = emitterGo.transform.parent.gameObject;
            if (activeEmitterMap.ContainsKey(actor)) {
                var eventMap = activeEmitterMap[actor];
                eventMap.Remove(emitter.audioEvent);
            }
        }

        // detach from actor
        emitterGo.transform.parent = null;

        // remove event handlers
        emitter.onDone.RemoveAllListeners();

        // add back to pool
        EmitterToPool(emitter);
    }
}
