using System.Collections;
using UnityEngine;

public class TurretAIController : BotBrain {
    [Header("Turret AI Config")]
    [Tooltip("how long to fire weapon")]
    [Range(0.25f, 5f)]
    public float fireDuration = 1f;
    [Tooltip("cooldown after firing weapon before we can fire again")]
    [Range(0.25f, 5f)]
    public float fireCooldown = 1f;
    [Tooltip("active range of turret")]
    public float targetRange = 15f;
    [Tooltip("angle to target must be +/- this angle to fire turret")]
    public float aimMinAngle = 5f;
    [Tooltip("how long to wait in between retarget cycles")]
    [Range(0.25f, 5f)]
    public float retargetInterval = 1f;
    public bool debug = false;

    [Header("State Variables")]
    public BotRuntimeSet currentBots;

    private GameObject target = null; // current target

    IEnumerator Target() {
        while (controlsActive) {
            // iterator through current bots
            if (currentBots != null) {
                for (var i=0; i<currentBots.Items.Count; i++)  {
                }
            }

            // attempt to retarget every second
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Aim() {
        yield return null;
    }

    IEnumerator Fire() {
        yield return null;
    }

    void Awake() {
    }

    void Update () {
    }

}
