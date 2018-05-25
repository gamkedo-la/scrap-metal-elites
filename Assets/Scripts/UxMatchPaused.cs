using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class UxMatchPaused : UxPanel {
    [Header("UI Reference")]
    public Button concedeButton;
    public Button optionsButton;
    public Button helpButton;
    public Button continueButton;

    [Header("Prefabs")]
    public GameObject optionsPanelPrefab;
    public GameObject helpPanelPrefab;

    [Header("Game Events")]
    public StringEvent onSelection;     // callback event to trigger when player clicks OK

    [Header("Config")]
    public bool debug = false;

    // enter state engine upon start
    void Start() {
        // display the modal
        Display();

        // hookup button callbacks
        concedeButton.onClick.AddListener(OnConcedeClick);
        optionsButton.onClick.AddListener(OnOptionsClick);
        helpButton.onClick.AddListener(OnHelpClick);
        continueButton.onClick.AddListener(OnContinueClick);
    }

    public void OnConcedeClick() {
        onSelection.Raise("concede");
        Hide();
    }

    public void OnOptionsClick() {
        // instantiate scores prefab (under canvas)
        Instantiate(optionsPanelPrefab, UxUtil.GetCanvas().gameObject.transform);
    }

    public void OnHelpClick() {
        // instantiate scores prefab (under canvas)
        Instantiate(helpPanelPrefab, UxUtil.GetCanvas().gameObject.transform);
    }

    public void OnContinueClick() {
        onSelection.Raise("continue");
        Hide();
    }

}
