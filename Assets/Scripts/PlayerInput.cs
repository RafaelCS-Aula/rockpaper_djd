using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : CharacterMovement
{
    private InputSettings iS;

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

        if (isController) iS.SwitchInput();
    }

    #region Update Methods

    void Update()
    {
        UpdateMovementAxis();
        UpdateVelocityFactor();
        UpdateJump();
        UpdateRotation();

        if (Input.GetKeyDown(iS.switchShoulders))
            camRig.SwitchShoulders();


        camRig.RotateCamera(
            !isController ? Input.GetAxisRaw(iS.hCamAxis) :
            -Input.GetAxisRaw(iS.hCamAxis),
            -Input.GetAxisRaw(iS.vCamAxis));
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
                velocityFactor = MovementSettings.diagonalVelocityFactor * MovementSettings.fallingVelocityFactor;
            else
                velocityFactor = MovementSettings.diagonalVelocityFactor;
        }
        else if (!controller.isGrounded)
            velocityFactor = MovementSettings.fallingVelocityFactor;

        else velocityFactor = MovementSettings.walkVelocityFactor;

    }


    private void UpdateJump()
    {
        if (controller.isGrounded && Input.GetKeyDown(iS.jump)) isJumping = true;
    }

    private void UpdateRotation()
    {
        float rotation = isController ?
            -Input.GetAxisRaw(iS.hCamAxis) * CameraSettings.camXSens :
            Input.GetAxisRaw(iS.hCamAxis) * CameraSettings.camXSens;

        transform.Rotate(0f, rotation, 0f);
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
