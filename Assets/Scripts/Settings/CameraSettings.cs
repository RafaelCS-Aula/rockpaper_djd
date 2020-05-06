using UnityEngine;

public static class CameraSettings
{
    [Header("-Positioning-")]
    public static Vector3 camPositionOffsetLeft = new Vector3(-1.0f, -0.3f, -4.0f);
    public static Vector3 camPositionOffsetRight = new Vector3(1.0f, -0.3f, -4.0f);

    [Header("-Camera Options-")]
    public static float camXSens = 5.0f;
    public static float camYSens = 5.0f;
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