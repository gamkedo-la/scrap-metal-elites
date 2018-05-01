// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
//
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "stringEvent", menuName = "Events/String")]
public class StringEvent : GameEvent {
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<StringEventListener> eventListeners =
        new List<StringEventListener>();

    public void Raise(string message)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--) {
            eventListeners[i].OnEventRaised(message);
        }
    }

    public void RegisterListener(StringEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(StringEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
