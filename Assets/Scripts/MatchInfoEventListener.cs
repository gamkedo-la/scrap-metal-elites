// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
//
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityMatchInfoEvent : UnityEvent<MatchInfo> {};

public class MatchInfoEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public MatchInfoEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityMatchInfoEvent Response;

    public void Awake() {
        if (Response == null) {
            Response = new UnityMatchInfoEvent();
        }
    }

    public void SetEvent(MatchInfoEvent newEvent) {
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

    public void OnEventRaised(MatchInfo message)
    {
        Response.Invoke(message);
    }
}
