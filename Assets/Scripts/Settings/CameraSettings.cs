using UnityEngine;

public static class CameraSettings
{
    [Header("-Positioning-")]
    public static Vector3 camPositionOffsetLeft;
    public static Vector3 camPositionOffsetRight;

    [Header("-Camera Options-")]
    public static Camera UICamera;
    public static float mouseXSensitivity = 5.0f;
    public static float mouseYSensitivity = 5.0f;
    public static float minAngle = -30.0f;
    public static float maxAngle = 70.0f;
    public static float rotationSpeed = 5.0f;
    public static float maxCheckDist = 0.1f;

    [Header("-Zoom-")]
    public static float fieldOfView = 70.0f;

    [Header("-Visual Options-")]
    public static float hideMeshWhenDistance = 0.5f;

    [Header("-Visual Options-")]
    public static float movementLerpSpeed = 5.0f;
}