using System.Collections;
using UnityEngine;

public enum SpotlightMode {
    None,
    Follow,
    Ballyhoo,
}

public class SpotlightAIController : MonoBehaviour {
    [Header("External References")]
    [Tooltip("the gameobject that acts as the yoke (handle) to the spotlight")]
    public GameObject yoke;

    [Header("Spotlight AI Config")]
    [Tooltip("active range of turret")]
    public float targetRange = 25f;
    [Tooltip("how long to wait in between retarget cycles")]
    [Range(0.25f, 5f)]
    public float retargetInterval = 1f;
    [Range(.001f, 1f)]
    [Tooltip("for follow mode, how fast does the light track to target")]
    public float followDampTime = 0.1f;
    [Tooltip("for ballyhoo mode, how fast does the light track to target")]
    public float ballyhooDampTime = 0.03f;
    [Tooltip("for ballyhoo mode, how large of an area should spotlight sweep")]
    public float ballyhooArea = 15f;
    [Tooltip("for ballyhoo mode, where is the floor/y-axis")]
    public float ballyhooFloor = -4f;
    [Tooltip("for ballyhoo mode, randomize quadrant vs. circle")]
    public bool ballyhooRandom = false;
    [Tooltip("starting spotlight mode")]
    public SpotlightMode startingMode = SpotlightMode.None;
    public bool debug = false;

    [Header("State Variables")]
    public BotRuntimeSet currentBots;
    public bool controlsActive = true;

    private GameObject target = null; // current target
    private Quaternion desiredRotation;
    private SpotlightMode spotlightMode = SpotlightMode.None;
    public float dampTime = 0.1f;

    public void StartBallyhoo() {
        spotlightMode = SpotlightMode.Ballyhoo;
        dampTime = ballyhooDampTime;
        StartCoroutine(Ballyhoo());
    }

    public void StartFollow() {
        spotlightMode = SpotlightMode.Follow;
        dampTime = followDampTime;
        StartCoroutine(Target());
        StartCoroutine(Aim());
    }

    public void Stop() {
        spotlightMode = SpotlightMode.None;
    }

    IEnumerator Target() {
        while (spotlightMode == SpotlightMode.Follow) {
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
        while (spotlightMode == SpotlightMode.Follow) {
            if (target != null) {
                desiredRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                //Debug.Log("desiredRotation: " + desiredRotation)
            }
            yield return null;  // wait until next frame
        }
    }

    IEnumerator Ballyhoo() {
        var quadrant = 0;
        while (spotlightMode == SpotlightMode.Ballyhoo) {
            // pick target location
            var x = Random.Range(0f,ballyhooArea/2f);
            var z = Random.Range(0f,ballyhooArea/2f);
            // adjust per quadrant
            switch (quadrant) {
            case 1:
                z = -z;
                break;
            case 2:
                x = -x;
                z = -z;
                break;
            case 3:
                x = -x;
                break;
            }
            var position = new Vector3(transform.position.x + x, ballyhooFloor, transform.position.z + z);
            // random mode... pick next quadrant at random
            if (ballyhooRandom) {
                int newQuad = quadrant;
                while (newQuad == quadrant) {
                    newQuad = Random.Range(0, 4);
                }
                quadrant = newQuad;
            // otherwise... circle mode, pick next quadrant in circle (clockwise)
            } else {
                quadrant++;
                quadrant %= 4;
            }
            desiredRotation = Quaternion.LookRotation(position - transform.position);
            // wait until we are within certain angle of desired rotation before picking next quadrant
            yield return new WaitUntil(() => Quaternion.Angle(transform.rotation, desiredRotation) < 30f);
        }
    }

    void Awake() {
        if (startingMode == SpotlightMode.Ballyhoo) {
            StartBallyhoo();
        } else if (startingMode == SpotlightMode.Follow) {
            StartFollow();
        }
    }

    void Rotate() {
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, dampTime);
    }

    void LateUpdate() {
        Rotate();
    }

}
