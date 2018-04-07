using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    [System.Serializable]
    public class OnHealthValueEvent : UnityEvent<int> { };
    public class OnDeathEvent : UnityEvent<GameObject> { };

    public int maxHealth = 100;
    public bool debug;
    public OnHealthValueEvent onChange;
    public OnHealthValueEvent onChangePercent;
    public OnDeathEvent onDeath;

    private int health;

    void Awake() {
        onChange = new OnHealthValueEvent();
        onChangePercent = new OnHealthValueEvent();
        onDeath = new OnDeathEvent();
        health = maxHealth;
        // cannot have zero or negative max health
        if (maxHealth < 1) {
            maxHealth = 1;
        }
    }

    public void TakeDamage(int amount, GameObject from) {
        if (debug) {
            Debug.Log("Taking damage: " + amount + " from: " + from);
        }
        // if health is already zero (or zero damage), can't take more damage
        if (health == 0 || amount <= 0) return;

        // apply damage
        health -= amount;

        // check for death
        if (health <= 0) {
            health = 0;
            onDeath.Invoke(from);
            if (debug) {
                Debug.Log("Death from: " + from);
            }
        }

        // communicate health change
        onChange.Invoke(health);
        var percent = (health*100)/maxHealth;
        onChangePercent.Invoke(percent);
    }

}
