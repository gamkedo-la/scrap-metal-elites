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

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(GameRecord record)
    {
        Response.Invoke(record);
    }
}
