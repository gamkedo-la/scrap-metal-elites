using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "steeringJoint", menuName = "Joints/Steering")]
public class SteeringJointApplicator : ComponentApplicator {
    public bool applyBreakForce;
    public FloatReference breakForce;
    public bool applyBreakTorque;
    public FloatReference breakTorque;

    public float steeringTorque;
    public float steeringDamper;
    public float maxTurnAngle;

    const string reverseSteeringTagKey = "steering.reverse";
    public static ConfigTag reverseSteeringTag;

    void OnEnable() {
        var guids = AssetDatabase.FindAssets("t:ConfigTag " + reverseSteeringTagKey);
        if (guids.Length > 0) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            reverseSteeringTag = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConfigTag)) as ConfigTag;
        }
    }

    public override void Apply(PartConfig config, GameObject target) {
        if (target == null) return;

        // find rigid body gameobject under target
        var rigidbodyGo = PartUtil.GetBodyGo(target);
        if (rigidbodyGo == null) return;

        // add fixed joint component to target
        var joint = rigidbodyGo.AddComponent<HingeJoint>();

        // add steering actuator
        var steering = rigidbodyGo.AddComponent<SteeringActuator>();
        if (reverseSteeringTag != null && config != null && config.Get<bool>(reverseSteeringTag)) {
            steering.reverse = true;
        } else {
            steering.reverse = false;
        }
        steering.maxTurnAngle = maxTurnAngle;

        // apply break limits, as specified
        if (applyBreakForce && breakForce != null) {
            joint.breakForce = breakForce.Value;
        }
        if (applyBreakTorque && breakTorque != null) {
            joint.breakTorque = breakTorque.Value;
        }

        // apply hinge properties
        joint.axis = new Vector3(0,1,0);
        var limits = joint.limits;
        limits.min = -maxTurnAngle;
        limits.max = maxTurnAngle;
        joint.limits = limits;
        joint.useLimits = true;
        var hingeSpring = joint.spring;
        hingeSpring.spring = steeringTorque;
        hingeSpring.damper = steeringDamper;
        hingeSpring.targetPosition = 0;
        joint.spring = hingeSpring;
        joint.useSpring = true;

        // add simple joiner script to target, allowing quick joint join
        var joiner = target.AddComponent<Joiner>();
        joiner.joint = joint;
    }
}
