using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class UxMatchPaused : UxPanel {
    [Header("UI Reference")]
    public Button concedeButton;
    public Button continueButton;

    [Header("Game Events")]
    public StringEvent onSelection;     // callback event to trigger when player clicks OK

    [Header("Config")]
    public bool debug = false;

    // enter state engine upon start
    void Start() {
        // display the modal
        Display();
        // wait for OK button to be clicked
        StartCoroutine(StateWait());
    }

    IEnumerator StateWait() {
        if (debug) Debug.Log("StateWait");
        string selection = "";

        // hookup button callbacks
        concedeButton.onClick.AddListener(() => {selection = "concede";});
        continueButton.onClick.AddListener(() => {selection = "continue";});

        // wait for button to be clicked
        yield return new WaitUntil(() => selection != "");

        // trigger confirmed
        onSelection.Raise(selection);

        // clean up event handler for button
        concedeButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        Hide();
    }

}
