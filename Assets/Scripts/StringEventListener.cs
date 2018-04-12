// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
//
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityStringEvent : UnityEvent<string> {};

public class StringEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public StringEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityStringEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(string message)
    {
        Response.Invoke(message);
    }
}
