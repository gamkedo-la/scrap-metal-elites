using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement {
    // ------------------------------------------------------------------------------
    // PROPERTIES
    float forwardDrive {get; set;}
    float rotateDrive {get; set;}
}

public interface IMotorActuator {
    // ------------------------------------------------------------------------------
    // PROPERTIES
    float forwardDrive {get; set;}
    bool left {get;}
}

public interface ISteeringActuator {
    // ------------------------------------------------------------------------------
    // PROPERTIES
    float rotateDrive {get; set;}
}
