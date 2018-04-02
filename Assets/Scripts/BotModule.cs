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

        if (label == null || label == "") {
            label = name;
        }

        // for a bot, we want to attach rigidbody for frame directly to root
        if (root != null) {
            if (!root.GetComponent<Rigidbody>() != null) {
                root.AddComponent<Rigidbody>();
            }
            bodyGo = root;
            // empty parts object to parent the rest of the bot
            partsGo = PartUtil.BuildGo(config, null, label + ".parts");
            partsGo.transform.position = root.transform.position;
            partsGo.transform.rotation = root.transform.rotation;
            // keep track of sibling
            var sibling = root.AddComponent<Sibling>();
            sibling.siblingGo = partsGo;

        // otherwise, instantiate rigidbody/parts as a normal part
        } else {
            // build out frame model first
            partsGo = base.Build(config, root, label);
            // set local position to match that of root
            bodyGo = partsGo.transform.Find(partsGo.name + ".body").gameObject;
            // FIXME
        }

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
                    if (moduleGo != null) {
                        // connect module to the frame
                        var joiner = moduleGo.GetComponent<Joiner>();
                        if (joiner != null) {
                            joiner.Join(bodyGo.GetComponent<Rigidbody>());
                        }
                    }
                }
            }
        }

        return partsGo;
    }
}
