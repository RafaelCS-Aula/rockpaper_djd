using UnityEngine;

public class PlayerInput : CharacterMovement
{
    private InputSettings iS;
    private ShooterBehaviour sB;

    [SerializeField] private CameraRig camRig;

    [SerializeField] private bool isController = false;



    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        iS = gameObject.AddComponent<InputSettings>();
        sB = GetComponent<ShooterBehaviour>();

        if (isController) iS.SwitchToController();
    }

    #region Update Methods

    void Update()
    {
        UpdateMovementAxis();
        UpdateVelocityFactor();
        UpdateJump();
        UpdateRotation();
        UpdateWeapon();
        UpdateCamera();


    }

    #region Movement Updates

    private void UpdateMovementAxis()
    {
        strafeAxis = Input.GetAxis(iS.hMovAxis);

        forwardAxis = Input.GetAxis(iS.vMovAxis);
    }
    private void UpdateVelocityFactor()
    {
        if (Input.GetAxis(iS.hMovAxis) != 0 && Input.GetAxis(iS.vMovAxis) != 0)
        {
            if (!controller.isGrounded)
                velocityFactor = movementSettings.diagonalVelocityFactor * movementSettings.fallingVelocityFactor;
            else
                velocityFactor = movementSettings.diagonalVelocityFactor;
        }
        else if (!controller.isGrounded)
            velocityFactor = movementSettings.fallingVelocityFactor;

        else velocityFactor = movementSettings.walkVelocityFactor;

    }
    private void UpdateJump()
    {
        if (controller.isGrounded && Input.GetKeyDown(iS.jump)) isJumping = true;
    }
    private void UpdateRotation()
    {
        float rotation = (isController ? -Input.GetAxisRaw(iS.hCamAxis) :
            Input.GetAxisRaw(iS.hCamAxis)) * camRig.cameraSettings.camXSens;

        transform.Rotate(0f, rotation, 0f);
    }

    #endregion

    private void UpdateCamera()
    {
        camRig.RotateCamera(-Input.GetAxisRaw(iS.vCamAxis));

        if (Input.GetKeyDown(iS.switchShoulders))
            camRig.SwitchShoulders();
    }

    private void UpdateWeapon()
    {
        if (Input.GetKey(iS.shoot))
        {
            sB.Shoot();
        }

        if (!isController)
        {
            if (Input.GetAxis(iS.typeScrollAxis) > 0f) sB.SelectWeapon(-1);
            else if (Input.GetAxis(iS.typeScrollAxis) < 0f) sB.SelectWeapon(1);
        }
        else
        {
            if (Input.GetKeyDown(iS.previousType)) sB.SelectWeapon(-1);
            if (Input.GetKeyDown(iS.nextType)) sB.SelectWeapon(1);
        }
    }

    #endregion

    private void ToogleCursor()
    {
        switch (Cursor.visible)
        {
            case true:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case false:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}
