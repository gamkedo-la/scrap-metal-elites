using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityPlayerInfoEvent : UnityEvent<PlayerInfo> {};

public class PlayerInfoEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public PlayerInfoEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityPlayerInfoEvent Response;

    public void Awake() {
        if (Response == null) {
            Response = new UnityPlayerInfoEvent();
        }
    }

    public void SetEvent(PlayerInfoEvent newEvent) {
        // unregister current event
        if (this.Event != null) {
            this.Event.UnregisterListener(this);
        }
        // register new
        if (newEvent != null) {
            this.Event = newEvent;
            this.Event.RegisterListener(this);
        }
    }

    private void OnEnable() {
        if (Event != null) {
            Event.RegisterListener(this);
        }
    }

    private void OnDisable() {
        if (Event != null) {
            Event.UnregisterListener(this);
        }
    }

    public void OnEventRaised(PlayerInfo message)
    {
        Response.Invoke(message);
    }
}
