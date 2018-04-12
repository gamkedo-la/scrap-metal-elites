using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UxBannerController: MonoBehaviour {
    public Text textField;

    public void OnFade(string message) {
        textField.GetComponent<CanvasRenderer>().SetAlpha(1f);
        textField.text = message;
        textField.CrossFadeAlpha(0.0f, 1.0f, false); //we want to make the image completely transparent
    }

    public void OnMessage(string message) {
        textField.GetComponent<CanvasRenderer>().SetAlpha(1f);
        textField.text = message;
    }

    public void OnClear() {
        textField.GetComponent<CanvasRenderer>().SetAlpha(1f);
        textField.text = "";
    }

    public void Start() {
        OnClear();
    }
}
