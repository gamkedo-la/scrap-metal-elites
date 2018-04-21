using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Simple Bot Drive Controller
// Requires IMovement component in same game object
public class BotBrain : MonoBehaviour {
    protected IMovement mover;
    protected IActuator weapon;

    public bool controlsActive = true;

    [Header("State Variables")]
    public BotRuntimeSet botList;
    public GameRecordEvent eventChannel;

    private bool botAlive = true;
    private bool flipped = false;

    void OnBotDeath(GameObject from) {
        botAlive = false;
    }

	// Use this for initialization
	void Start () {
        mover = GetComponent<IMovement>();
        weapon = GetComponent<IActuator>();
        if (eventChannel == null) {
            eventChannel = PartUtil.GetEventChannel(gameObject);
            Debug.Log("eventChannel: " + eventChannel);
        }
        // determine when bot flips
        StartCoroutine(DetectFlip());

        // determine when bot dies
        var botHealth = GetComponent<BotHealth>();
        if (botHealth != null) {
            botHealth.onDeath.AddListener(OnBotDeath);
        }
	}

    public void OnGameRecord(GameRecord record) {
        Debug.Log(record.ToString());
    }

    IEnumerator DetectFlip() {
        while (botAlive) {
            var currentFlip = Vector3.Angle(Vector3.up, transform.up) > 90;
            if (currentFlip != flipped) {
                Debug.Log("just flipped");
                flipped = currentFlip;
                if (eventChannel != null) {
                    eventChannel.Raise(GameRecord.BotFlipped(gameObject));
                }
            }
            // wait until next frame;
            yield return null;
        }
    }

}
