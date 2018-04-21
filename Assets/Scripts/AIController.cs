using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIMood {
    idle,
    wander,
    aggressive,
    flee
};

public class AIController : BotBrain {
    public GameObject target;
    public float thinkSpeed = 1.0f;
    public AIMood moodNow;

	// Use this for initialization
	void Awake() {
		// assign player tag
		var rootGo = PartUtil.GetRootGo(gameObject);
		if (rootGo != null) {
			rootGo.tag = "enemy";
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

}
