using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotHealth : MonoBehaviour {
    [Tooltip("when combined module health falls below this percentage, bot dies")]
    [Range(1,75)]
    public int deathHealthPercent = 50;

    public OnHealthValueEvent onChange;
    public OnHealthValueEvent onChangePercent;
    public OnDeathEvent onDeath;

    private bool runDiscover = true;
    private List<Health> healthModules;
    private int totalHealth = 0;
    private int minHealth;

    // bot health is computed as the combined health of the bot modules
    private int health {
        get {
            int v = 0;
            if (healthModules != null) {
                for (var i=0; i<healthModules.Count; i++) {
                    v += healthModules[i].health;
                }
            }
            return v;
        }
    }

    private int healthPercent {
        get {
            var currentHealth = health;
            if (currentHealth > minHealth) {
                return ((currentHealth-minHealth)*100)/(totalHealth-minHealth);
            } else {
                return 0;
            }
        }
    }

    void Discover() {
        totalHealth = 0;
        healthModules = new List<Health>();
        var components = PartUtil.GetComponentsInChildren<Health>(gameObject);
        for (var i=0; i<components.Length; i++) {
            // only consider bot modules for overall bot health
            if (components[i].healthTag == HealthTag.Module) {
                totalHealth += components[i].maxHealth;
                healthModules.Add(components[i]);
                components[i].onChange.AddListener(OnHealthChange);
            }
        }
        minHealth = (totalHealth * deathHealthPercent)/100;
        Debug.Log(gameObject.name + " # healthModules: " + healthModules.Count + " totalHealth: " + totalHealth + " minHealth: " + minHealth);
    }

    void Update() {
        if (runDiscover) {
            Discover();
            runDiscover = false;
        }
    }

    void OnHealthChange(int newValue) {
        Debug.Log("bot on health change, current health: " + health + " percent: " + healthPercent);
        onChange.Invoke(health);
        onChangePercent.Invoke(healthPercent);
        if (healthPercent <= 0) {
            onDeath.Invoke(null);
            Debug.Log("death");
        }
    }

}
