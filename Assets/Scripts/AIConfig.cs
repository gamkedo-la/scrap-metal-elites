using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIMode {
    idle,
    wander,
    aggressive,
    flee,
    hitAndRun
};

public enum TargetMode {
    Manual,
    Closest,
    Player,
}

[CreateAssetMenu(fileName = "aiConfig", menuName = "AI/Config")]
public class AIConfig : ScriptableObject {
    [Header("modes")]
    [Tooltip("ai mode to start with")]
    public AIMode startingMode;
    [Tooltip("target mode to start with")]
    public TargetMode startingTargetMode = TargetMode.Manual;

    [Header("drive params")]
    [Tooltip("how close should we try to get to target")]
    [Range(0.01f, 5f)]
    public float driveRange = 2f;
	[MinMaxRange(0, 2)]
	public RangedFloat drivePulseRange;
	[MinMaxRange(0, 1)]
	public RangedFloat driveCooldown;
    [Tooltip("drive steering angle factor")]
    [Range(5f, 90f)]
    public float driveSteeringFactor = 10f;
    [Tooltip("after within aimMinAngle, delay before steering again")]
    [Range(0f, 2f)]
    public float steeringDelay = 0f;
    [Tooltip("clamp of steering drive")]
    [Range(0f, 1f)]
    public float steeringPowerModifier = 1f;
    [Tooltip("does bot use track steering")]
    public bool steeringTrack = true;
    [Tooltip("for track steering, min angle to engage drive")]
    [Range(0f, 90f)]
    public float minDriveAngle = 15f;

    [Header("weapon firing params")]
    [Tooltip("angle to target must be +/- this angle to fire weapon")]
    [Range(1f, 25f)]
    public float aimMinAngle = 10f;
    [Tooltip("how long to fire weapon")]
    [Range(0.01f, 5f)]
    public float fireDuration = .25f;
    [Tooltip("cooldown after firing weapon before we can fire again")]
    [Range(0.25f, 5f)]
    public float fireCooldown = 1f;
    [Tooltip("minimum range to target before firing weapon")]
    public float fireRange = 2.5f;

    [Header("targeting params")]
    [Tooltip("for target closest mode, active range of targeting")]
    public float targetRange = 25f;
    [Tooltip("how long to wait in between retarget cycles")]
    [Range(0.25f, 5f)]
    public float retargetInterval = 1f;

    [Header("State Variables")]
    public BotRuntimeSet currentBots;

    [Header("other params")]
    [Range(1, 5f)]
    public float fleeDuration = 3f;
    [Range(0f, 5f)]
    public float fleeDistanceThreshold = 3f;
    public bool debug = false;
}
