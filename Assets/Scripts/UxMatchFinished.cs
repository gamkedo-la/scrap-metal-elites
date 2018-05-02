using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class UxMatchFinished : MonoBehaviour {
    [Header("UI Reference")]
    public Text messageText;
    public Button okButton;
    public CanvasGroup canvasGroup;

    [Header("Game Events")]
    public GameEvent onConfirmed;     // callback event to trigger when player clicks OK

    [Header("Config")]
    public bool debug = false;

    private string[] winningStrings = {
        "All the base are belong to {0}",
        "{0} was in the base, killing the d00dz",
        "In life, there are no winners or losers...\r\nbut {0} totally won",
        "#{0}\r\n#Winning",
        "{0} was too legit to quit",
        "{0} was the winner we deserved",
    };

    private string[] losingStrings = {
        "{0}, you really need to try harder!",
        "I'm sorry {0}, you lose."
    };

    public void OnWantFinishConfirmation(string msg) {
        Debug.Log("OnWantFinishConfirmation: " + msg);
        // display the modal
        Display();

        // parse message into win/loss:player_name
        string playerName = "idk";
        bool playerWon = false;
        if (msg != "") {
            var fields = msg.Split(':');
            if (fields.Length == 2) {
                playerWon = fields[0] == "win";
                playerName = fields[1];
            }
        }

        // format popup message
        string popupMessage;
        if (playerWon) {
            var index = UnityEngine.Random.Range(0, winningStrings.Length);
            popupMessage = System.String.Format(winningStrings[index], playerName);
        } else {
            var index = UnityEngine.Random.Range(0, losingStrings.Length);
            popupMessage = System.String.Format(losingStrings[index], playerName);
        }
        if (messageText != null) {
            messageText.text = popupMessage;
        }

        // wait for OK button to be clicked
        StartCoroutine(StateWaitOk());
    }


    IEnumerator StateWaitOk() {
        if (debug) Debug.Log("StateWaitOK");
        bool okClicked = false;
        // create match info event listener to listen for callback event from bot select modal
        okButton.onClick.AddListener(()=>{okClicked = true;});

        // wait for match info to be selected
        yield return new WaitUntil(() => okClicked);

        // trigger confirmed
        if (onConfirmed != null) {
            onConfirmed.Raise();
        }

        // clean up event handler for button
        okButton.onClick.RemoveAllListeners();
        Hide();

    }

    void Start() {
        // start out w/ modal hidden
        Hide();
    }

    void Display() {
        if (canvasGroup != null) {
            canvasGroup.alpha = 1f; //this makes everything transparent
            canvasGroup.blocksRaycasts = true; //this prevents the UI element to receive input events
            canvasGroup.interactable = true;
        }
    }

    void Hide() {
        if (canvasGroup != null) {
            canvasGroup.alpha = 0f; //this makes everything transparent
            canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
            canvasGroup.interactable = false;
        }
    }
}
