using UnityEngine;

namespace rockpaper_djd
{
    [CreateAssetMenu(fileName = "CameraData",
    menuName = "Data/Settings/Camera Data", order = 0)]
    public class CameraData : ScriptableObject
    {
        [Header("-Positioning-")]
        [Tooltip("Offset vector between the camera and the character's eyes")]
        public Vector3 cameraOffset;

        [Header("-Camera Options-")]
        [Tooltip("X Axis Sensitivity")]
        public float camXSens;

        [Tooltip("Y Axis Sensitivity")]
        public float camYSens;

        [Tooltip("Minimum angle between the camera and the character's eyes")]
        public float minAngle;

        [Tooltip("Maximum angle between the camera and the character's eyes")]
        public float maxAngle;

        [Tooltip("Maximum distance for checking if the player is close to a wall")]
        public float maxCheckDist;

        [Header("-Zoom-")]
        [Tooltip("Field Of View for the camera")]
        public float fieldOfView;

        [Header("-Visual Options-")]
        [Tooltip("Maximum distance allowed between" +
            " the player and the camera, until the player's layer" +
            " is disable from the camera's culling mask")]
        public float hideMeshWhenDistance;

        [Tooltip("Speed at which the camera will be lerped to the new position")]
        public float movementLerpSpeed;
    }
}