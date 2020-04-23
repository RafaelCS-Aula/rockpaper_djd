using UnityEngine;

public static class MovementSettings
{
    [Header("-Acceleration Options-")]
    public static float maxAcceleration        = 80.0f;
    public static float jumpAcceleration       = 500.0f;
    public static float gravityAcceleration    = 20.0f;

    [Header("-Velocity Options-")]
    public static float maxForwardVelocity     = 5.0f;
    public static float maxBackwardVelocity    = 4.0f;
    public static float maxStrafeVelocity      = 4.0f;
    public static float maxJumpVelocity        = 50.0f;
    public static float maxFallVelocity        = 5.0f;

    [Header("-Factor Options-")]
    public static float walkVelocityFactor     = 1.0f;

    public static float diagonalVelocityFactor = 0.5f;
    public static float fallingVelocityFactor  = 0.9f;
}