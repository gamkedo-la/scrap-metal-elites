using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    public GameObject target;
    public float thinkSpeed = 1.0f;
    private Movement moveScript;
    public enum mood { idle, wander, aggressive, flee };
    public mood moodNow;

	// Use this for initialization
	void Start () {
        moveScript = gameObject.GetComponent<Movement>();
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
        float angToFaceTarget = 0.0f;
        if (target) {
            float angToTarget = Mathf.Atan2(target.transform.position.z - transform.position.z,
                target.transform.position.x - transform.position.x);
            angToFaceTarget = AngleAroundAxis(transform.forward, Quaternion.AngleAxis(angToTarget * Mathf.Rad2Deg, Vector3.up) * Vector3.forward, Vector3.up);
            Debug.Log(angToFaceTarget);
        }
        switch (moodNow) {
            case mood.idle:
                break;
            case mood.wander:
                moveScript.forwardDrive = 0.0f;
                moveScript.rotateDrive = -1.0f;
                break;
            case mood.aggressive:
                moveScript.forwardDrive = 0.0f;
                moveScript.rotateDrive = (angToFaceTarget < 0.0f ? -1.0f : 1.0f);

                break;
            case mood.flee:
                break;
        }

    }

}
