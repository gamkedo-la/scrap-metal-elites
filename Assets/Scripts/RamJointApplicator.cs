using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ramJoint", menuName = "Joints/Ram")]
public class RamJointApplicator : JointApplicator {
    [Tooltip("How much force to bring ram back to starting position")]
    public float springForce = 200f;
    [Tooltip("Damper for spring")]
    public float springDamper = 5f;
    [Tooltip("Ram motor force")]
	public float motorForce = 1000.0f;
    [Tooltip("Damper for motor")]
    public float motorDamper = .5f;
    [Tooltip("How much additional force to apply to target on collision")]
	public float impactForce = 400.0f;
    [Tooltip("multiply computed damage")]
	public float impactDamageMultiplier = 1f;
    [Tooltip("Expected max ram speed")]
	public float maxRamSpeed = 400.0f;
    [Tooltip("Axis the ram will operate on X:0, Y:1, Z:2")]
    public int axis = 0; // 0 => x, 1 => y, 2 => z
    [Tooltip("Max traverse distance")]
    public float maxTraverse = 1f;
    public bool debug = false;

    protected override Joint ApplyJoint(GameObject rigidbodyGo, PartConfig config, GameObject target) {

        // add joint component to target
        var joint = rigidbodyGo.AddComponent<ConfigurableJoint>();
        joint.xMotion = (axis == 0) ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
        joint.yMotion = (axis == 1) ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
        joint.zMotion = (axis == 2) ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
        var linearLimit = joint.linearLimit;
        linearLimit.limit = maxTraverse;
        joint.linearLimit = linearLimit;
        var linearLimitSpring = joint.linearLimitSpring;
        linearLimitSpring.spring = springForce;
        linearLimitSpring.damper = springDamper;
        joint.linearLimitSpring = linearLimitSpring;
        JointDrive drive;
        // setup drive based on axis
        if (axis == 2) {
            drive = joint.zDrive;
        } else if (axis == 1) {
            drive = joint.yDrive;
        } else {
            drive = joint.xDrive;
        }
        drive.positionSpring = motorForce;
        drive.positionDamper = motorDamper;
        drive.maximumForce = 10000;
        if (axis == 2) {
            joint.zDrive = drive;
        } else if (axis == 1) {
            joint.yDrive = drive;
        } else {
            joint.xDrive = drive;
        }

        // add actuator
        var actuator = rigidbodyGo.AddComponent<RamActuator>();
        actuator.motorForce = motorForce;
        actuator.impactForce = impactForce;
        actuator.impactDamageMultiplier = impactDamageMultiplier;
        actuator.maxRamSpeed = maxRamSpeed;
        actuator.axis = axis;
        actuator.maxTraverse = maxTraverse;
        actuator.debug = debug;
        return joint;

    }
}
