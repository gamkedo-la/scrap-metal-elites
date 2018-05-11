using UnityEngine;

public class OnOffAIController : BotBrain {
    [Range(0.25f, 5f)]
    public float delay = 1f;
    public bool debug = false;
    public bool randomize = false;

    private bool activated = false;
    private float currentTimer = 0f;
    private float currentDelay = 0f;

    void Awake() {
        currentDelay = delay;
        // FIXME: remove
        //EnableControls();
    }

    void Update () {
        if (!controlsActive) return;

        // trigger state change based on timer delay
        currentTimer += Time.deltaTime;
        if (currentTimer > currentDelay) {
            activated = (activated) ? false : true;
            if (debug) Debug.Log("timer flip, activated: " + activated);
            if (randomize) currentDelay = Random.Range(0f, delay);
            currentTimer = 0f;
        }

        // trigger weapon on/off
        if (weapon != null) {
            weapon.actuate = activated ? 1f : 0f;
        }

    }

}
