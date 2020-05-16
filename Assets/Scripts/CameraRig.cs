using System;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    #region Serializable Classes

    [Serializable]
    public class CameraSettings
    {
        [Header("-Positioning-")]
        public Vector3 cameraOffset = new Vector3(1.0f, -0.3f, -4.0f);

        [Header("-Camera Options-")]
        public float camXSens = 5.0f;
        public float camYSens = 5.0f;
        public float minAngle = -30.0f;
        public float maxAngle = 70.0f;
        public float rotationSpeed = 5.0f;
        public float maxCheckDist = 0.1f;

        [Header("-Zoom-")]
        public float fieldOfView = 70.0f;

        [Header("-Visual Options-")]
        public float hideMeshWhenDistance = .5f;

        [Header("-Visual Options-")]
        public float movementLerpSpeed = 5.0f;
    }

    [SerializeField]
    public CameraSettings cameraSettings;

    #endregion

    #region Class Setup

    [SerializeField]
    private Transform target;

    public LayerMask wallLayers;
    public LayerMask playerLayer;

    private Camera cam;
    private Vector3 currentOffset;

    float newY = 0.0f;

    public Transform Pivot { get; set; }

    void Start()
    {
        Pivot = transform.GetChild(0);

        cam = Pivot.GetComponentInChildren<Camera>();
        cam.fieldOfView = cameraSettings.fieldOfView;

        currentOffset = cameraSettings.cameraOffset;
    }

    #endregion

    #region Late Update Methods

    void LateUpdate()
    {
        FollowTarget();
    }

    //Following the target with Time.deltaTime smoothly
    private void FollowTarget()
    {
        transform.position = target.position;
    }

    #endregion

    #region Update Methods

    // Update is called once per frame
    void Update()
    {
        if (!cam || !Pivot || !target) return;

        if (Application.isPlaying)
        {
            CheckforWalls();
            ToggleMeshVisibility();
            CheckPlayerinFrontofCamera();
        }

    }

    //Checks the wall and moves the camera if there's a collision
    private void CheckforWalls()
    {
        Vector3 direction = cam.transform.position - Pivot.position;

        float distance = Mathf.Abs(currentOffset.z);

        if (Physics.SphereCast(Pivot.position, cameraSettings.maxCheckDist,
            direction, out RaycastHit hit, distance, wallLayers))
        {
            MoveCameraForward(hit, direction);
        }

        else PositionCamera(currentOffset);
    }

    //This moves the camera forward when it hits a wall
    private void MoveCameraForward(RaycastHit hit,
        Vector3 direction)
    {
        Vector3 sphereCastCenter = Pivot.position +
            (direction.normalized * hit.distance);

        cam.transform.position = sphereCastCenter;
    }

    /// <summary>
    /// Transitions the camera from it's current position,
    /// to the new given position
    /// </summary>
    /// <param name="newPosition">Camera's new position</param>
    private void PositionCamera(Vector3 newPosition)
    {
        //Postions the cameras localPosition to a given location
        cam.transform.localPosition =
            Vector3.Lerp(cam.transform.localPosition, newPosition,
            Time.deltaTime * cameraSettings.movementLerpSpeed);
    }

    /// <summary>
    /// Hides the mesh targets mesh renderers when too close
    /// </summary>
    private void ToggleMeshVisibility()
    {
        // Calculates distance from the camere to the player mesh
        float distance = Vector3.Distance(cam.transform.position, target.position + target.up * 2);

        // Checks if the distance between the two is less than or equal to the distance allowed
        if (distance <= cameraSettings.hideMeshWhenDistance)
        {
            // Unchecks "Player" layer to the camera's culling mask
            cam.cullingMask &= ~(playerLayer);
        }

        // Checks "Player" layer to the camera's culling mask otherwise
        else cam.cullingMask |= playerLayer;
    }

    #endregion

    #region Input Methods

    //Rotates the camera with input
    public void RotateCamera(float camYAxis)
    {

        newY += cameraSettings.camYSens * camYAxis;

        Vector3 eulerAngleAxis = new Vector3 { x = newY, y = 0 };

        newY = Mathf.Clamp(newY, cameraSettings.minAngle, cameraSettings.maxAngle);

        Pivot.localRotation = Quaternion.Euler(eulerAngleAxis);
    }

    /// <summary>
    /// Switches the camera's shoulder view
    /// </summary>
    public void SwitchShoulders()
    {
        currentOffset.x *= -1;
    }


    private void CheckPlayerinFrontofCamera()
    {
        //Debug.DrawRay(cam.transform.position, cam.transform.forward * 100.0f, Color.yellow);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward,
            1000, playerLayer))
        {
            cam.cullingMask &= ~(playerLayer.value);
        }

        else cam.cullingMask |= playerLayer.value;
    }

    #endregion
}

