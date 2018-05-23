using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : BotBrain {
    public AIConfig config;

    private TargetMode targetMode = TargetMode.Manual;
    private AIMode currentMode;
    private float angleToTarget = 0f;
    private float distanceToTarget = 0f;
    private GameObject target;
    private bool fleeing = false;

    public void AssignConfig(AIConfig defaultConfig) {
        // look up config, attached to bot
        var builder = GetComponent<BotBuilder>();
        if (builder != null && builder.aiConfig != null) {
            config = builder.aiConfig;
        } else {
            config = defaultConfig;
        }
    }

    //------------------------------------------------------------------------------
    // Target modes
    //------------------------------------------------------------------------------
    public void TargetManual(GameObject target) {
        this.target = target;
        if (controlsActive) {
            targetMode = TargetMode.Manual;
        } else {
            config.startingTargetMode = TargetMode.Manual;
        }
    }

    public void TargetClosest() {
        if (controlsActive) {
            targetMode = TargetMode.Closest;
            StartCoroutine(StateTargetClosest());
        } else {
            config.startingTargetMode = TargetMode.Closest;
        }
    }

    public void TargetPlayer() {
        this.target = GameObject.FindWithTag("player");
        if (controlsActive) {
            targetMode = TargetMode.Player;
        } else {
            config.startingTargetMode = TargetMode.Player;
        }
    }

    IEnumerator Move() {
        var activeTime = 0f;
        var nextPulse = Random.Range(config.drivePulseRange.minValue, config.drivePulseRange.maxValue);
        while (controlsActive) {
            if (target != null) {
                activeTime += Time.deltaTime;
                if (activeTime < nextPulse) {
                    // scale drive based on distance to target and min driveRange
                    float drive = 0f;
                    // if fleeing, reverse drive, get out
                    if (fleeing) {
                        drive = -1f;
                    // if track steering, don't engage forward drive if angleToTarget past threshold
                    } else if (config.steeringTrack && Mathf.Abs(angleToTarget) >= 15f) {
                        drive = 0f;
                    } else {
                        drive = distanceToTarget/config.driveRange - 1f;
                    }
                    if (flipped) {
                        drive = -drive;
                    }
                    mover.forwardDrive = Mathf.Clamp(drive, -1f, 1f);
                } else {
                    activeTime = 0f;
                    mover.forwardDrive = 0f;
                    yield return new WaitForSeconds(Random.Range(config.driveCooldown.minValue, config.driveCooldown.maxValue));
            		nextPulse = Random.Range(config.drivePulseRange.minValue, config.drivePulseRange.maxValue);
                }
            }
            yield return null;  // wait until next frame
        }
    }

    IEnumerator Steer() {
        var activeTime = 0f;
        while (controlsActive) {
            if (mover != null && target != null) {
                activeTime += Time.deltaTime;
                if (activeTime >= config.steeringDelay) {
                    // scale drive, if within min angle scale drive down
                    var drive = angleToTarget/config.driveSteeringFactor;
                    // flip controls if bot is flipped
                    if (flipped) {
                        drive = -drive;
                    }
                    // rotate towards target
                    //mover.rotateDrive = Mathf.Clamp(drive, -1f, 1f);
                    mover.rotateDrive = Mathf.Clamp(drive, -config.steeringPowerModifier, config.steeringPowerModifier);

                    // if within target aim angle, restart active time
                    if (Mathf.Abs(angleToTarget) < config.aimMinAngle) {
                        activeTime = 0f;
                    }
                }
            }
            yield return null;  // wait until next frame
        }
    }

    IEnumerator Fire() {
        while (controlsActive) {
            if (weapon != null && target != null && Mathf.Abs(angleToTarget) < config.aimMinAngle && distanceToTarget<config.fireRange) {
                // fire at target
                weapon.actuate = 1f;
                yield return new WaitForSeconds(config.fireDuration);
                weapon.actuate = 0f;
                // cooldown
                yield return new WaitForSeconds(config.fireCooldown);
            }
            yield return null;  // wait until next frame
        }
    }

    void SetTargetingMode(TargetMode mode) {
        if (mode == TargetMode.Closest) {
            TargetClosest();
        } else if (mode == TargetMode.Player) {
            TargetPlayer();
        } else {
            TargetManual(null);
        }
    }

    void SetAIMode(AIMode mode) {
        currentMode = mode;
        if (mode == AIMode.hitAndRun) {
            StartCoroutine(StateHitAndRun());
        } if (mode == AIMode.flee) {
            fleeing = true;
        }
    }

    public override void EnableControls() {
        base.EnableControls();
        SetTargetingMode(config.startingTargetMode);
        SetAIMode(config.startingMode);
        StartCoroutine(Fire());
        StartCoroutine(Steer());
        StartCoroutine(Move());
    }

	// Use this for initialization
	void Awake() {
		// assign player tag
		var rootGo = PartUtil.GetRootGo(gameObject);
		if (rootGo != null) {
			rootGo.tag = "enemy";
		}
	}

    void Update () {
        if (target != null) {
            angleToTarget = AIController.AngleAroundAxis(transform.forward, target.transform.position-transform.position, transform.up);
            distanceToTarget = (target.transform.position-transform.position).magnitude;
        }
    }

    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        // Project A and B onto the plane orthogonal to axis
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        // Find (positive) angle between A and B
        float angle = Vector3.Angle(dirA, dirB);

        // Return angle multiplied with 1 or -1
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    } // via https://forum.unity.com/threads/turn-left-or-right-to-face-a-point.22235/

    // Update is called once per frame
    // FIXME: integrate modes into new steer/drive/fire routines
    /*
    void Update ()
    {
        // are controls active?
        if (!controlsActive) return;
        float angToFaceTarget = 0.0f;
        if (target) {
			float angToTarget = Mathf.Atan2(target.transform.position.x - transform.position.x,
				target.transform.position.z - transform.position.z);
            angToFaceTarget = AngleAroundAxis(transform.forward, Quaternion.AngleAxis(angToTarget * Mathf.Rad2Deg, Vector3.up) * Vector3.forward, Vector3.up);
            //Debug.Log("angle to target: " +angToFaceTarget);
        }
        switch (moodNow) {
            case AIMood.idle:
                break;
            case AIMood.wander:
                mover.forwardDrive = 1.0f;
                mover.rotateDrive = -1.0f;
                break;
            case AIMood.aggressive:
                mover.forwardDrive = 1.0f;
                if (Mathf.Abs(angToFaceTarget) >= 10f) {
                    var targetDrive = (angToFaceTarget < 0.0f ? -1.0f : 1.0f);
                    mover.rotateDrive = targetDrive;
                } else {
                    mover.rotateDrive = 0f;
                }
                break;
            case AIMood.flee:
                mover.forwardDrive = -1.0f;
                if (Mathf.Abs(angToFaceTarget) >= 10f) {
                    mover.rotateDrive = (angToFaceTarget < 0.0f ? -1.0f : 1.0f);
                } else {
                    mover.rotateDrive = 0f;
                }
                break;
        }

    }
        */

    IEnumerator StateHitAndRun() {
        fleeing = false;
        yield return null;  // wait until next frame
        while (controlsActive && currentMode == AIMode.hitAndRun) {
            if (distanceToTarget <= config.fleeDistanceThreshold) {
                if (config.debug) Debug.Log(gameObject.name + " fleeing");
                fleeing = true;
                yield return new WaitForSeconds(config.fleeDuration);
            }
            yield return null;  // wait until next frame
            if (config.debug && fleeing) Debug.Log(gameObject.name + " no longer fleeing");
            fleeing = false;
        }
    }

    IEnumerator StateTargetClosest() {
        while (controlsActive && targetMode == TargetMode.Closest) {
            GameObject closest = null;
            float distance = 0f;
            // iterator through current bots, find the closest
            if (config.currentBots != null) {
                for (var i=0; i<config.currentBots.Items.Count; i++)  {
                    // skip self
                    if (config.currentBots.Items[i].gameObject == gameObject) continue;
                    // evaluate for closest
                    if (closest == null || (config.currentBots.Items[i].gameObject.transform.position-transform.position).magnitude < distance) {
                        closest = config.currentBots.Items[i].gameObject;
                        distance = (closest.transform.position-transform.position).magnitude;
                    }
                }
            }
            // if closest is within range, set as target
            if (closest != null && distance < config.targetRange) {
                if (target != closest) {
                    target = closest;
                    if (config.debug) Debug.Log(gameObject.name + " setting target to: " + target.name);
                }
            } else {
                target = null;
                if (config.debug) Debug.Log(gameObject.name + " clearing target");
            }
            // attempt to retarget on given retarget interval
            yield return new WaitForSeconds(config.retargetInterval);
        }
    }

}
