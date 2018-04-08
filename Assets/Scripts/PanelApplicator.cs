using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "panel", menuName = "Parts/Panel")]
public class PanelApplicator : ComponentApplicator {
    public PartReference top;
    public PartReference bottom;
    public PartReference left;
    public PartReference right;
    public PartReference front;
    public PartReference back;

    public override void Apply(PartConfig config, GameObject target) {
        // find rigid body gameobject under target
        if (target == null) return;
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // apply panels based on config
        if (top != null && config != null && config.Get<bool>(ConfigTag.PanelTop)) {
            top.Build(config, rigidbodyGo, "top");
        }
        if (bottom != null && config != null && config.Get<bool>(ConfigTag.PanelBottom)) {
            bottom.Build(config, rigidbodyGo, "bottom");
        }
        if (left != null && config != null && config.Get<bool>(ConfigTag.PanelLeft)) {
            left.Build(config, rigidbodyGo, "left");
        }
        if (right != null && config != null && config.Get<bool>(ConfigTag.PanelRight)) {
            right.Build(config, rigidbodyGo, "right");
        }
        if (front != null && config != null && config.Get<bool>(ConfigTag.PanelFront)) {
            front.Build(config, rigidbodyGo, "front");
        }
        if (back != null && config != null && config.Get<bool>(ConfigTag.PanelBack)) {
            back.Build(config, rigidbodyGo, "back");
        }
    }

}
