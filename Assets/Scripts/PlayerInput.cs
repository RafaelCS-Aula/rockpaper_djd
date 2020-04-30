using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : CharacterMovement
{

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    #region Update Methods

    void Update()
    {
        UpdateMovementAxis();
        UpdateVelocityFactor();
        UpdateJump();
        UpdateRotation();
    }

    private void UpdateMovementAxis()
    {
        strafeAxis = Input.GetAxis("Strafe");

        forwardAxis = Input.GetAxis("Forward");
    }

    private void UpdateVelocityFactor()
    {
        if (Input.GetAxis("Strafe") != 0 && Input.GetAxis("Forward") != 0)
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
        if (controller.isGrounded && Input.GetKeyDown(InputSettings.jump)) isJumping = true;
    }

    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * CameraSettings.mouseXSensitivity;

        transform.Rotate(0f, rotation, 0f);
    }

    #endregion

    private void ToogleCursor()
    {
        if (Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }
}
