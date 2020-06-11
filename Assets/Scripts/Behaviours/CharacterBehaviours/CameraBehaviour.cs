using System;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour, IDataUser<CameraData>
{
    #region Data Handling

    [SerializeField] private CameraData _dataFile;
    public CameraData DataHolder
    {
        get => _dataFile;
        set => value = _dataFile;
    }

    private Vector3 cameraOffset;

    public float camXSens;
    public float camYSens;
    public float minAngle;
    public float maxAngle;
    public float rotationSpeed;
    public float maxCheckDist;

    public float fieldOfView;

    public float hideMeshWhenDistance;
    public float movementLerpSpeed;

    public void GetData()
    {
        cameraOffset = DataHolder.cameraOffset;

        camXSens = DataHolder.camXSens;
        camYSens = DataHolder.camYSens;
        minAngle = DataHolder.minAngle;
        maxAngle = DataHolder.maxAngle;
        rotationSpeed = DataHolder.rotationSpeed;
        maxCheckDist = DataHolder.maxCheckDist;

        fieldOfView = DataHolder.fieldOfView;

        hideMeshWhenDistance = DataHolder.hideMeshWhenDistance;
        movementLerpSpeed = DataHolder.movementLerpSpeed;
    }

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


    void Awake()
    {
        if (DataHolder == null)
        {
            Debug.LogError("Object doesn't have a Data Scriptable Object assigned.");
            throw new UnityException();
        }

        GetData();
    }

    void Start()
    {
        Pivot = transform.GetChild(0);

        cam = Pivot.GetComponentInChildren<Camera>();
        cam.fieldOfView = fieldOfView;

        currentOffset = cameraOffset;

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
        Ray r = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(r.origin, r.direction * 1000, Color.yellow);

    }

    //Checks the wall and moves the camera if there's a collision
    private void CheckforWalls()
    {
        Vector3 direction = cam.transform.position - Pivot.position;

        float distance = Mathf.Abs(currentOffset.z);

        if (Physics.SphereCast(Pivot.position, maxCheckDist,
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
            Time.deltaTime * movementLerpSpeed);
    }

    /// <summary>
    /// Hides the target's mesh renderers when the mesh
    /// covers the center of the camera
    /// </summary>
    private void ToggleMeshVisibility()
    {
        float distance = Vector3.Distance(cam.transform.position, target.position + target.up * 2);


        if (Physics.Raycast(cam.transform.position, cam.transform.forward,
        1000, playerLayer) || distance <= hideMeshWhenDistance)
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

        newY += camYSens * camYAxis;

        Vector3 eulerAngleAxis = new Vector3 { x = newY, y = 0 };

        newY = Mathf.Clamp(newY, minAngle, maxAngle);

        Pivot.localRotation = Quaternion.Euler(eulerAngleAxis);
    }

    /// <summary>
    /// Switches the camera's shoulder view
    /// </summary>
    public void SwitchShoulders()
    {
        currentOffset.x *= -1;
    }

    #endregion

    #region coordinate method

    public Vector3 GetCenterTarget()
    {

        float range = 1000;
        Ray r = new Ray(cam.transform.position + cam.transform.forward * 100, cam.transform.forward);
        Debug.DrawRay(r.origin, r.direction * range, Color.yellow);

        /*bool r = Physics.Raycast(
         cam.transform.position, cam.transform.forward, out RaycastHit hit); */
        //RaycastHit hit = Physics.Raycast(r, 10000.0f, layerMask: target.gameObject.layer);

        bool hitTarget = Physics.Raycast(r, out RaycastHit hitInf, range);

        if (hitTarget) return hitInf.point;

        else return cam.transform.forward * range;
    }

    #endregion
}

