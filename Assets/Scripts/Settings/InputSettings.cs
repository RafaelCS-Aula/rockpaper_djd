using System;
using UnityEngine;

[ExecuteInEditMode]
public class InputSettings : MonoBehaviour
{
    #region Keyboard and Mouse Input

    public KeyCode switchShoulders = KeyCode.V;

    [Header("Combat Controls")]
    public KeyCode shoot = KeyCode.Mouse0;

    public KeyCode switchToRock = KeyCode.Alpha1;
    public KeyCode switchToPaper = KeyCode.Alpha2;
    public KeyCode switchToScissors = KeyCode.Alpha3;

    public KeyCode previousType = KeyCode.JoystickButton4;  //For Controller
    public KeyCode nextType= KeyCode.JoystickButton5;  //For Controller
    public string typeScrollAxis = "Mouse ScrollWheel"; //For Mouse&Keyboard

    [Header("Movement Controls")]
    public string hMovAxis = "KeyboardHorizontal";
    public string vMovAxis = "KeyboardVertical";

    public KeyCode jump = KeyCode.Space;
    public KeyCode dash = KeyCode.LeftShift;

    [Header("Camera Controls")]
    public string hCamAxis = "Mouse X";
    public string vCamAxis = "Mouse Y";

    #endregion

    public void SwitchToController()
    {
        hMovAxis = "LeftJoystickHorizontal";
        vMovAxis = "LeftJoystickVertical";
        jump = KeyCode.JoystickButton1;
        dash = KeyCode.JoystickButton6;

        hCamAxis = "RightJoystickHorizontal";
        vCamAxis = "RightJoystickVertical";
        switchShoulders = KeyCode.JoystickButton10;

        shoot = KeyCode.JoystickButton7;
    }
}