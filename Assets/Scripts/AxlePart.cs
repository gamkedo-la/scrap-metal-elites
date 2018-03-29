using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "steeredWheelAssembly", menuName = "Parts/steeredWheelAssembly")]
public class AxlePart : Part {
    public ModelReference frame;
    public ModelReference kingpin;
    public ModelReference hub;

    public override void Display(
        IDisplayer displayer
    ) {
        if (frame != null) {
            frame.Display(displayer);
        }
        if (hub != null) {
            hub.Display(displayer);
        }
        if (kingpin != null) {
            kingpin.Display(displayer);
        }
        base.Display(displayer);
    }

    /*
    public GameObject Build(
        GameObject root,
        string label
    ) {
        if (label == null || label == "") {
            label = name;
        }

        // create empty parts container
        var partsGo = new GameObject(label);
        partsGo.transform.parent = root.transform;

        // create new rigid body for this part, set parts container as parent
        var rigidbodyGo = new GameObject(label + ".body", typeof(Rigidbody));
        rigidbodyGo.transform.parent = partsGo.transform;

        return partsGo;
    }
    */
}
