using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class OnHealthValueEvent : UnityEvent<int> { };
[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { };

public class Health : MonoBehaviour {

    [System.Serializable]
    public class IntValueEvent : UnityEvent<int> { };
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { };
    [System.Serializable]
    public class GameObjectIntEvent : UnityEvent<GameObject, int> { };

    public int maxHealth = 100;
    public bool debug;
    public HealthTag healthTag = HealthTag.Part;
    public IntValueEvent onChange;
    public IntValueEvent onChangePercent;
    public GameObjectEvent onDeath;
    public GameObjectIntEvent onTakeDamage;

    public int health {
        get {
            return _health;
        }
    }

    private int _health;

    void Awake() {
        onChange = new IntValueEvent();
        onChangePercent = new IntValueEvent();
        onDeath = new GameObjectEvent();
        onTakeDamage = new GameObjectIntEvent();
    }
    void Start() {
        _health = maxHealth;
        // cannot have zero or negative max health
        if (maxHealth < 1) {
            maxHealth = 1;
        }
    }

    public void TakeDamage(int amount, GameObject from) {
        if (debug) {
            Debug.Log(name + " taking damage: " + amount + " from: " + from);
        }
        // if health is already zero (or zero damage), can't take more damage
        if (_health == 0 || amount <= 0) return;

        // apply damage
        _health -= amount;

        // check for death
        if (_health <= 0) {
            _health = 0;
            onDeath.Invoke(from);
            if (debug) {
                Debug.Log("Death from: " + from);
            }
        }

        // communicate health change
        onChange.Invoke(_health);
        var percent = (_health*100)/maxHealth;
        onChangePercent.Invoke(percent);
        onTakeDamage.Invoke(from, amount);
    }

}
