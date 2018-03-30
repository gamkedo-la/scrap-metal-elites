using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class PartConfigMap {
    public string label;
    public PartConfig config;
    public PartReference part;
}

[CreateAssetMenu(fileName = "bot", menuName = "Bot")]
public class BotModule: Part {
    public ModelReference frame;
    public PartConfigMap[] modules;

    public override GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject partsGo = null;
        GameObject bodyGo = null;
        GameObject steeringBodyGo = null;
        GameObject hubBodyGo = null;
        Vector3 rotation = Vector3.zero;
        Vector3 offset = Vector3.zero;

        // build out frame model first
        partsGo = base.Build(config, root, label);
        bodyGo = partsGo.transform.Find(partsGo.name + ".body").gameObject;

        // frame is instantiated under parts.body
        if (frame != null) {
            frame.Build(config, bodyGo, "frame");
        }

        // now build out modules
        if (modules != null) {
            for (var i=0; i<modules.Length; i++) {
                if (modules[i] != null && modules[i].part != null) {
                    // build the module under the top level parts
                    var moduleGo = modules[i].part.Build(modules[i].config, partsGo, modules[i].label);

                    // connect module to the frame
                    var joiner = moduleGo.GetComponent<Joiner>();
                    if (joiner != null) {
                        joiner.Join(bodyGo.GetComponent<Rigidbody>());
                    }
                }
            }
        }

        return partsGo;
    }
}
