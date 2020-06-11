using UnityEngine;

[CreateAssetMenu(fileName = "CameraData",
    menuName = "Data/Settings/Camera Data", order = 0)]
public class CameraData : ScriptableObject
{
    [Header("-Positioning-")]
    [Tooltip("Offset vector between the camera and the character's eyes")]
    public Vector3 cameraOffset = new Vector3(1f, 1f, -3f);

    [Header("-Camera Options-")]
    [Tooltip("X Axis Sensitivity")]
    public float camXSens = 4f;

    [Tooltip("Y Axis Sensitivity")]
    public float camYSens = 3f;

    [Tooltip("Minimum angle between the camera and the character's eyes")]
    public float minAngle = -30f;

    [Tooltip("Maximum angle between the camera and the character's eyes")]
    public float maxAngle = 70f;

    [Tooltip("Speed at which the camera rotates around the target")]
    public float rotationSpeed = 5f;

    [Tooltip("Maximum distance for checking if the player is close to a wall")]
    public float maxCheckDist = 0.2f;

    [Header("-Zoom-")]
    [Tooltip("Field Of View for the camera")]
    public float fieldOfView = 70.0f;

    [Header("-Visual Options-")]
    [Tooltip("Maximum distance allowed between" +
        " the player and the camera, until the player's layer" +
        " is disable from the camera's culling mask")]
    public float hideMeshWhenDistance = 1f;

    [Tooltip("Speed at which the camera will be lerped to the new position")]
    public float movementLerpSpeed = 5.0f;
}
