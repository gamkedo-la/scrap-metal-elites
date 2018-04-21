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
    [Range(1f, 15f)]
    public float aimMinAngle = 5f;
    [Tooltip("how long to wait in between retarget cycles")]
    [Range(0.25f, 5f)]
    public float retargetInterval = 1f;
    public bool debug = false;

    [Header("State Variables")]
    public BotRuntimeSet currentBots;

    private GameObject target = null; // current target
    private bool runDiscovery = true;
    private GameObject turretGo = null;
    private float angleToTarget = 0f;

    IEnumerator Target() {
        while (controlsActive) {
            GameObject closest = null;
            float distance = 0f;

            // iterator through current bots, find the closest
            if (currentBots != null) {
                for (var i=0; i<currentBots.Items.Count; i++)  {
                    if (closest == null || (currentBots.Items[i].gameObject.transform.position-transform.position).magnitude < distance) {
                        closest = currentBots.Items[i].gameObject;
                        distance = (closest.transform.position-transform.position).magnitude;
                    }
                }
            }

            // if closest is within range, set as target
            if (closest != null && distance < targetRange) {
                if (target != closest) {
                    target = closest;
                    if (debug) Debug.Log(gameObject.name + " setting target to: " + target.name);
                }
            } else {
                target = null;
                if (debug) Debug.Log(gameObject.name + " clearing target");
            }

            // attempt to retarget on given retarget interval
            yield return new WaitForSeconds(retargetInterval);
        }
    }

    IEnumerator Aim() {
        while (controlsActive) {
            if (mover != null && target != null && turretGo != null) {
                // compute angle to target
                angleToTarget = AIController.AngleAroundAxis(turretGo.transform.forward, target.transform.position-turretGo.transform.position, turretGo.transform.up);
                // scale drive, if within min angle scale drive down
                var drive = angleToTarget/aimMinAngle;
                // rotate towards target
                mover.rotateDrive = Mathf.Clamp(drive, -1f, 1f);
            }
            yield return null;  // wait until next frame
        }
    }

    IEnumerator Fire() {
        while (controlsActive) {
            if (weapon != null && target != null && Mathf.Abs(angleToTarget) < aimMinAngle) {
                // fire at target
                weapon.actuate = 1f;
                yield return new WaitForSeconds(fireDuration);
                weapon.actuate = 0f;
                // cooldown
                yield return new WaitForSeconds(fireCooldown);
            }
            yield return null;  // wait until next frame
        }
    }

    void Awake() {
        StartCoroutine(Target());
        StartCoroutine(Aim());
        StartCoroutine(Fire());
    }

    void Discover() {
        var turretActuator = PartUtil.GetComponentInChildren<TurretActuator>(gameObject);
        if (turretActuator != null) {
            turretGo = turretActuator.gameObject;
            Debug.Log("turretGo: " + turretGo);
        }
    }

    void Update () {
        if (runDiscovery) {
            Discover();
            runDiscovery = false;
        }
    }

}
