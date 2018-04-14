using UnityEngine;

public class HammerAIController : BotBrain {
    [Range(0.25f, 5f)]
    public float delay = 1f;
    public bool debug = false;

    private bool activated = false;
    private float currentTimer = 0f;

    void Update () {
        if (!controlsActive) return;

        // trigger state change based on timer delay
        currentTimer += Time.deltaTime;
        if (currentTimer > delay) {
            activated = (activated) ? false : true;
            if (debug) Debug.Log("timer flip, activated: " + activated);
            currentTimer = 0f;
        }

        // trigger weapon on/off
        if (weapon != null) {
            weapon.actuate = activated ? 1f : 0f;
        }

    }

}
