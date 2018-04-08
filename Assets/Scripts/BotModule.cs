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
    [Tooltip("Mesh/Collider model/prefab to be associated with module frame, null means no model will be displayed for module frame")]
    public ModelReference frame;
    [Tooltip("modules to add to the bot")]
    public PartConfigMap[] modules;

    public override GameObject Build(
        PartConfig config,
        GameObject root,
        string label
    ) {
        GameObject partsGo = null;
        GameObject bodyGo = null;
        Vector3 rotation = Vector3.zero;
        Vector3 offset = Vector3.zero;

        if (label == null || label == "") {
            label = name;
        }

        // for a bot, we want to attach rigidbody for frame directly to root
        // only do this in-game, not for in-editor preview
        if (root != null && Application.isPlaying) {
            if (root.GetComponent<Rigidbody>() == null) {
                root.AddComponent<Rigidbody>();
            }
            bodyGo = root;
            if (mass != null) {
                mass.Apply(config, root);
            }
            //PartUtil.ApplyRigidBodyProperties(bodyGo, mass, drag, angularDrag);
            // empty parts object to parent the rest of the bot
            partsGo = PartUtil.BuildGo(config, null, label + ".parts");
            partsGo.transform.position = root.transform.position;
            partsGo.transform.rotation = root.transform.rotation;
            // keep track of parent/child links
            var childLink = root.AddComponent<ChildLink>();
            childLink.childGo = partsGo;
            var parentLink = partsGo.AddComponent<ParentLink>();
            parentLink.parentGo = root;

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
                    var moduleGo = modules[i].part.Build(PartConfig.Merge(modules[i].config, config), partsGo, modules[i].label);
                    if (moduleGo != null) {
                        var moduleBodyGo = PartUtil.GetBodyGo(moduleGo);
                        if (moduleBodyGo != null) {
                            // connect module to the
                            var joiner = moduleBodyGo.GetComponent<Joiner>();
                            if (joiner != null) {
                                joiner.Join(bodyGo.GetComponent<Rigidbody>());
                            }
                        }
                    }
                }
            }
        }

        return partsGo;
    }
}
