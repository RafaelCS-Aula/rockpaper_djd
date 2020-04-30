using UnityEngine;

public static class InputSettings
{
    [Header("Movement Controls")]
    public static KeyCode jump = KeyCode.Space;
    public static KeyCode dash = KeyCode.LeftShift;

    [Header("Camera Controls")]
    public static KeyCode switchShoulders = KeyCode.V;

    [Header("Combat Controls")]
    public static KeyCode shoot = KeyCode.Mouse0;
    public static KeyCode switchToRock = KeyCode.Alpha1;
    public static KeyCode switchToPaper = KeyCode.Alpha2;
    public static KeyCode switchToScissors = KeyCode.Alpha3;
}