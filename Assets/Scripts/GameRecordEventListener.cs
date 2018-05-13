// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
//
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameRecordEvent : UnityEvent<GameRecord> {};

public class GameRecordEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameRecordEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityGameRecordEvent Response;

    public void Awake() {
        if (Response == null) {
            Response = new UnityGameRecordEvent();
        }
    }

    public void SetEvent(GameRecordEvent newEvent) {
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

    private void OnEnable()
    {
        if (Event != null) {
            Event.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        if (Event != null) {
            Event.UnregisterListener(this);
        }
    }

    public void OnEventRaised(GameRecord record)
    {
        Response.Invoke(record);
    }
}
