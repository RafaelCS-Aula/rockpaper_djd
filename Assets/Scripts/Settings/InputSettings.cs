using System;
using UnityEngine;

[ExecuteInEditMode]
public class InputSettings : MonoBehaviour
{
    [Header("Movement Controls")]
    public string hMovAxis = "KeyboardHorizontal";
    public string vMovAxis = "KeyboardVertical";

    public KeyCode jump = KeyCode.Space;
    public KeyCode dash = KeyCode.LeftShift;


    [Header("Camera Controls")]
    public string hCamAxis = "Mouse X";
    public string vCamAxis = "Mouse Y";
    public KeyCode switchShoulders = KeyCode.V;


    [Header("Combat Controls")]
    public KeyCode shoot = KeyCode.Mouse0;

    public KeyCode previousType = KeyCode.JoystickButton4;
    public KeyCode nextType = KeyCode.JoystickButton5;  
    public string typeScrollAxis = "Mouse ScrollWheel";

    public KeyCode switchToRock = KeyCode.Alpha1;
    public KeyCode switchToPaper = KeyCode.Alpha2;
    public KeyCode switchToScissors = KeyCode.Alpha3;


    public void SetInputType(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Keyboard1:

                hMovAxis = "KeyboardHorizontal";
                vMovAxis = "KeyboardVertical";

                jump = KeyCode.Space;
                dash = KeyCode.LeftShift;

                hCamAxis = "Mouse X";
                vCamAxis = "Mouse Y";

                switchShoulders = KeyCode.V;

                shoot = KeyCode.Mouse0;

                typeScrollAxis = "Mouse ScrollWheel";

                switchToRock = KeyCode.Alpha1;
                switchToPaper = KeyCode.Alpha2;
                switchToScissors = KeyCode.Alpha3;

                break;

            case InputType.Keyboard2:

                hMovAxis = "KeyboardHorizontal";
                vMovAxis = "KeyboardVertical";

                jump = KeyCode.Space;
                dash = KeyCode.LeftShift;

                hCamAxis = "Mouse X";
                vCamAxis = "Mouse Y";

                switchShoulders = KeyCode.V;

                shoot = KeyCode.Mouse0;

                previousType = KeyCode.N;
                nextType = KeyCode.M;

                switchToRock = KeyCode.Alpha4;
                switchToPaper = KeyCode.Alpha5;
                switchToScissors = KeyCode.Alpha6;

                break;

            case InputType.PS4Controller:

                hMovAxis = "LeftJoystickHorizontal";
                vMovAxis = "LeftJoystickVertical";

                jump = KeyCode.JoystickButton1;
                dash = KeyCode.JoystickButton6;

                hCamAxis = "RightJoystickHorizontal";
                vCamAxis = "RightJoystickVertical";

                switchShoulders = KeyCode.JoystickButton10;

                shoot = KeyCode.JoystickButton7;

                previousType = KeyCode.JoystickButton4;
                nextType = KeyCode.JoystickButton5;

                break;

            default:
                break;
        }
    }
}