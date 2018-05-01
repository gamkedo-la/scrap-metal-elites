using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerInfoEvent", menuName = "Events/PlayerInfo")]
public class PlayerInfoEvent : GameEvent {
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<PlayerInfoEventListener> eventListeners =
        new List<PlayerInfoEventListener>();

    public void Raise(PlayerInfo message)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--) {
            eventListeners[i].OnEventRaised(message);
        }
    }

    public void RegisterListener(PlayerInfoEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(PlayerInfoEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
