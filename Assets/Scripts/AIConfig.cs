using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIMode {
    idle,
    wander,
    aggressive,
    flee
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
    [Tooltip("angle to target must be +/- this angle to fire weapon")]
    [Range(1f, 15f)]
    public float aimMinAngle = 10f;

    [Header("weapon firing params")]
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
    public bool debug = false;
}
