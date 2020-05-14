using UnityEngine;

public class CameraRig : MonoBehaviour
{
    #region Class Setup

    [SerializeField]
    private Transform target;

    public LayerMask wallLayers;

    public LayerMask playerLayer;

    private Camera cam;


    public enum Shoulder
    {
        Right, Left
    }

    private Shoulder shoulder;

    float newX = 0.0f;
    float newY = 0.0f;

    public Transform Pivot { get; set; }

    void Start()
    {
        Pivot = transform.GetChild(0);

        cam = Pivot.GetComponentInChildren<Camera>();
        cam.fieldOfView = CameraSettings.fieldOfView;
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
        }

    }

    //Checks the wall and moves the camera if there's a collision
    private void CheckforWalls()
    {
        Vector3 direction = cam.transform.position - Pivot.position;

        float distance = Mathf.Abs(shoulder == Shoulder.Left ?
            CameraSettings.camPositionOffsetLeft.z :
            CameraSettings.camPositionOffsetRight.z);

        if (Physics.SphereCast(Pivot.position, CameraSettings.maxCheckDist,
            direction, out RaycastHit hit, distance, wallLayers))
        {
            MoveCameraForward(hit, direction);
        }

        else
        {
            switch (shoulder)
            {
                case Shoulder.Left:
                    PostionCamera(CameraSettings.camPositionOffsetLeft);
                    break;
                case Shoulder.Right:
                    PostionCamera(CameraSettings.camPositionOffsetRight);
                    break;
            }
        }
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
    private void PostionCamera(Vector3 newPosition)
    {
        //Postions the cameras localPosition to a given location
        cam.transform.localPosition =
            Vector3.Lerp(cam.transform.localPosition, newPosition,
            Time.deltaTime * CameraSettings.movementLerpSpeed);
    }

    /// <summary>
    /// Hides the mesh targets mesh renderers when too close
    /// </summary>
    private void ToggleMeshVisibility()
    {
        // Calculates distance from the camere to the player mesh
        float distance = Vector3.Distance(cam.transform.position, target.position + target.up);

        // Checks if the distance between the two is less than or equal to the distance allowed
        if (distance <= CameraSettings.hideMeshWhenDistance)
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
    public void RotateCamera(float camXAxis, float camYAxis)
    {
        newX += CameraSettings.camXSens * camXAxis;
        newY += CameraSettings.camYSens * camYAxis;

        Vector3 eulerAngleAxis = new Vector3 { x = newY, y = newX };

        newX = Mathf.Repeat(newX, 360);
        newY = Mathf.Clamp(newY, CameraSettings.minAngle, CameraSettings.maxAngle);

        Pivot.localRotation = Quaternion.Euler(eulerAngleAxis);
    }

    /// <summary>
    /// Switches the camera's shoulder view
    /// </summary>
    public void SwitchShoulders()
    {
        // Checks the current value of the 'shoulder' variable
        switch (shoulder)
        {
            // In case that the current value is 'Shoulder.Left'
            case Shoulder.Left:
                // Sets the 'shoulder' value to 'Shoulder.Right'
                shoulder = Shoulder.Right;
                break;

            // In case that the current value is 'Shoulder.Right'
            case Shoulder.Right:
                // Sets the 'shoulder' value to 'Shoulder.Left'
                shoulder = Shoulder.Left;
                break;
        }
    }

    #endregion
}

