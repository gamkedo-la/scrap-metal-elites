using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "hubJoint", menuName = "Joints/Hub")]
public class HubJointApplicator : JointApplicator {
    public bool motor = true;
    public float motorMaxTorque;
    public float motorMaxSpeed;

    protected override Joint ApplyJoint(GameObject rigidbodyGo, PartConfig config, GameObject target) {
        var joint = rigidbodyGo.AddComponent<HingeJoint>();

        // add motor
        var applyMotor = motor;
        if (config != null && config.Has(ConfigTag.MotorEnable) && !config.Get<bool>(ConfigTag.MotorEnable)) {  // config override
            applyMotor = false;
        }
        if (applyMotor) {
            var motorActuator = rigidbodyGo.AddComponent<MotorActuator>();
            motorActuator.maxTorque = motorMaxTorque;
            motorActuator.maxSpeed = motorMaxSpeed;
            // apply motor.left from config
            if (config != null && config.Get<bool>(ConfigTag.MotorLeft)) {
                motorActuator.isLeft = true;
            }
            // apply motor.left from config
            if (config != null && config.Get<bool>(ConfigTag.MotorReverse)) {
                motorActuator.reverse = true;
            }
        }
        return joint;
    }

}
