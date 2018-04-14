using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameRecordTag {
    GamePrepared,
    GameStarted,
    GameFinished,
    BotTookDamage,
    BotJointBroke,
    BotDied,
}

[System.Serializable]
public class GameRecord {
    public GameRecordTag tag;
    public GameObject actor;
    public GameObject target;
    public int intValue;

    public GameRecord(
        GameRecordTag tag,
        GameObject actor,
        GameObject target,
        int intValue
    ) {
        this.tag = tag;
        this.actor = actor;
        this.target = target;
        this.intValue = intValue;
    }

    public override string ToString() {
        var str = "";
        str += String.Format("{0} - ", tag);
        if (actor != null) {
            str += String.Format("actor: {0} ", actor);
        }
        if (target != null) {
            str += String.Format("target: {0} ", target);
        }
        if (intValue != 0) {
            str += String.Format("value: {0} ", intValue);
        }
        return str;
    }

    public static GameRecord GamePrepared() {
        return new GameRecord(GameRecordTag.GamePrepared, null, null, 0);
    }
    public static GameRecord GameStarted() {
        return new GameRecord(GameRecordTag.GameStarted, null, null, 0);
    }
    public static GameRecord GameFinished() {
        return new GameRecord(GameRecordTag.GameFinished, null, null, 0);
    }
    public static GameRecord BotTookDamage(
        GameObject target,              // who took the damage
        GameObject actor,               // who/what inflicted the damage
        int damageAmount
    ) {
        return new GameRecord(GameRecordTag.BotTookDamage, actor, target, damageAmount);
    }
    public static GameRecord BotJointBroke(
        GameObject target
    ) {
        return new GameRecord(GameRecordTag.BotJointBroke, null, target, 0);
    }
    public static GameRecord BotDied(
        GameObject target,              // who died
        GameObject actor                // who/what caused death
    ) {
        return new GameRecord(GameRecordTag.BotDied, actor, target, 0);
    }

}

[CreateAssetMenu(fileName = "gameRecordEvent", menuName = "Events/GameRecord")]
public class GameRecordEvent : GameEvent {
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<GameRecordEventListener> eventListeners =
        new List<GameRecordEventListener>();

    public void Raise(GameRecord record)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(record);
    }

    public void RegisterListener(GameRecordEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameRecordEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
