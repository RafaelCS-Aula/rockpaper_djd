using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [System.Serializable]
    public class CamSettings
    {
        [Header("-Positioning-")]
        public Vector3 camPositionOffsetLeft;
        public Vector3 camPositionOffsetRight;

        [Header("-Camera Options-")]
        public Camera UICamera;
        public float mouseXSensitivity = 5.0f;
        public float mouseYSensitivity = 5.0f;
        public float minAngle = -30.0f;
        public float maxAngle = 70.0f;
        public float rotationSpeed = 5.0f;
        public float maxCheckDist = 0.1f;

        [Header("-Zoom-")]
        public float fieldOfView = 70.0f;

        [Header("-Visual Options-")]
        public float hideMeshWhenDistance = 0.5f;

        [Header("-Visual Options-")]
        public float movementLerpSpeed = 5.0f;
    }

    [SerializeField]
    CamSettings camSettings;


    private Transform target;

    public LayerMask wallLayers;

    public enum Shoulder
    {
        Right, Left
    }
    public Shoulder shoulder;

    float newX = 0.0f;
    float newY = 0.0f;

    public Transform pivot { get; set; }

    // Use this for initialization
    void Start()
    {
        pivot = transform.GetChild(0);

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }



    // Update is called once per frame
    void Update()
    {
        if (!Camera.main || !pivot || !target) return;

        if (Application.isPlaying)
        {
            RotateCamera();
            CheckforWalls();
            ToggleMeshVisibility();

            if (Input.GetKeyDown(InputSettings.switchShoulders))
                SwitchShoulders();
        }

    }

    void LateUpdate()
    {
        FollowTarget();
    }

    //Following the target with Time.deltaTime smoothly
    void FollowTarget()
    {
        transform.position = target.position;
    }

    //Rotates the camera with input
    void RotateCamera()
    {
        newX += camSettings.mouseXSensitivity * Input.GetAxis("Mouse X");
        newY += camSettings.mouseYSensitivity * -Input.GetAxis("Mouse Y");

        Vector3 eulerAngleAxis = new Vector3 { x = newY, y = newX };

        newX = Mathf.Repeat(newX, 360);
        newY = Mathf.Clamp(newY, camSettings.minAngle, camSettings.maxAngle);

        pivot.localRotation = Quaternion.Euler(eulerAngleAxis);
    }

    //Checks the wall and moves the camera if there's a collision
    void CheckforWalls()
    {
        Vector3 dir = Camera.main.transform.position - pivot.position;

        float distance = Mathf.Abs(shoulder == Shoulder.Left ?
            camSettings.camPositionOffsetLeft.z :
            camSettings.camPositionOffsetRight.z);

        if (Physics.SphereCast(pivot.position, camSettings.maxCheckDist,
            dir, out RaycastHit hit, distance, wallLayers))
        {
            MoveCameraForward(hit, dir);
        }

        else
        {
            switch (shoulder)
            {
                case Shoulder.Left:
                    PostionCamera(camSettings.camPositionOffsetLeft);
                    break;
                case Shoulder.Right:
                    PostionCamera(camSettings.camPositionOffsetRight);
                    break;
            }
        }
    }

    //This moves the camera forward when it hits a wall
    void MoveCameraForward(RaycastHit hit,
        Vector3 direction)
    {
        Vector3 sphereCastCenter = pivot.position +
            (direction.normalized * hit.distance);

        Camera.main.transform.position = sphereCastCenter;
    }

    /// <summary>
    /// Transitions the camera from it's current position,
    /// to the new given position
    /// </summary>
    /// <param name="newPosition">Camera's new position</param>
    void PostionCamera(Vector3 newPosition)
    {
        //Postions the cameras localPosition to a given location
        Camera.main.transform.localPosition =
            Vector3.Lerp(Camera.main.transform.localPosition, newPosition,
            Time.deltaTime * camSettings.movementLerpSpeed);
    }

    /// <summary>
    /// Hides the mesh targets mesh renderers when too close
    /// </summary>
    void ToggleMeshVisibility()
    {
        // Calculates distance from the camere to the player mesh
        float distance = Vector3.Distance(Camera.main.transform.position, target.position + target.up);

        // Checks if the distance between the two is less than or equal to the distance allowed
        if (distance <= camSettings.hideMeshWhenDistance)
            // Unchecks "Player" layer to the camera's culling mask
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));

        // Checks "Player" layer to the camera's culling mask otherwise
        else Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Player");
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
}
