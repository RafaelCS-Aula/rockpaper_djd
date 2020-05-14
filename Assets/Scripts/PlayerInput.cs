using UnityEngine;

public class PlayerInput : CharacterMovement
{
    private InputSettings    iS;
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
        iS = new InputSettings();
        sB = GetComponent<ShooterBehaviour>();

        if (isController) iS.SwitchInput();
    }

    #region Update Methods

    void Update()
    {
        UpdateMovementAxis();
        UpdateVelocityFactor();
        UpdateJump();
        UpdateRotation();
        UpdateShooting();


        camRig.RotateCamera(
            !isController ? Input.GetAxisRaw(iS.hCamAxis) :
            -Input.GetAxisRaw(iS.hCamAxis),
            -Input.GetAxisRaw(iS.vCamAxis));

        if (Input.GetKeyDown(iS.switchShoulders))
            camRig.SwitchShoulders();
    }

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
        float rotation = isController ?
            -Input.GetAxisRaw(iS.hCamAxis) * camRig.cameraSettings.camXSens :
            Input.GetAxisRaw(iS.hCamAxis) * camRig.cameraSettings.camXSens;

        transform.Rotate(0f, rotation, 0f);
    }

    private void UpdateShooting()
    {
        if (Input.GetKey(iS.shoot))
        {
            sB.Shoot();
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
