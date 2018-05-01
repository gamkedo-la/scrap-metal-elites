using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "matchInfoEvent", menuName = "Events/MatchInfo")]
public class MatchInfoEvent : GameEvent {
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<MatchInfoEventListener> eventListeners =
        new List<MatchInfoEventListener>();

    public void Raise(MatchInfo message)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--) {
            eventListeners[i].OnEventRaised(message);
        }
    }

    public void RegisterListener(MatchInfoEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(MatchInfoEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
