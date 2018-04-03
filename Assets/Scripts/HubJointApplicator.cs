using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "hubJoint", menuName = "Joints/Hub")]
public class HubJointApplicator : JointApplicator {
    public bool motor = true;
    public FloatReference motorMaxTorque;
    public FloatReference motorMaxSpeed;

    const string motorLeftTagKey = "motor.left";
    const string motorEnableTagKey = "motor.enable";
    public static ConfigTag motorLeftTag;
    public static ConfigTag motorEnableTag;

    void OnEnable() {
        var guids = AssetDatabase.FindAssets("t:ConfigTag " + motorLeftTagKey);
        if (guids.Length > 0) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            motorLeftTag = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConfigTag)) as ConfigTag;
        }
        guids = AssetDatabase.FindAssets("t:ConfigTag " + motorEnableTagKey);
        if (guids.Length > 0) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            motorEnableTag = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConfigTag)) as ConfigTag;
        }
    }

    protected override Joint ApplyJoint(GameObject rigidbodyGo, PartConfig config, GameObject target) {
        var joint = rigidbodyGo.AddComponent<HingeJoint>();

        // add motor
        var applyMotor = motor && (motorMaxTorque != null) && (motorMaxSpeed != null);
        if (config != null && config.Has(motorEnableTag) && !config.Get<bool>(motorEnableTag)) {  // config override
            applyMotor = false;
        }
        if (applyMotor) {
            var motorActuator = rigidbodyGo.AddComponent<MotorActuator>();
            motorActuator.maxTorque = motorMaxTorque;
            motorActuator.maxSpeed = motorMaxSpeed;
            // apply motor.left from config
            if (motorLeftTag != null && config != null && config.Get<bool>(motorLeftTag)) {
                motorActuator.isLeft = true;
            }
        }
        return joint;
    }
    
}