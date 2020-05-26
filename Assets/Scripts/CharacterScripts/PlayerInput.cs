using System;
using UnityEngine;

public class PlayerInput : CharacterMovement
{
    [SerializeField] private InputType inputType;

    [HideInInspector] public ShooterBehaviour sB;

    private InputSettings iS;
    private CameraRig camRig;

    [SerializeField] private GameObject rockIndicator;
    [SerializeField] private GameObject paperIndicator;
    [SerializeField] private GameObject scissorsIndicator;
    [SerializeField] private Material indicatorMaterial;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        audioHandler = GetComponent<PlayerSoundHandler>();
    }

    private void Start()
    {
        iS = gameObject.AddComponent<InputSettings>();
        iS.SetInputType(inputType);

        camRig = GetComponentInChildren<CameraRig>();
        sB = GetComponent<ShooterBehaviour>();

        indicatorMaterial.color = new Color(0, 244, 0, 0.1f);
    }

    #region Update Methods

    void Update()
    {
        UpdateMovementAxis();
        UpdateCamera();
        UpdateRotation();
        UpdateAMR();
        UpdateWeapon();
    }

    #region Movement Updates

    private void UpdateMovementAxis()
    {
        strafeAxis = Input.GetAxis(iS.hMovAxis);

        forwardAxis = Input.GetAxis(iS.vMovAxis);
    }

    private void UpdateCamera()
    {
        camRig.RotateCamera(-Input.GetAxisRaw(iS.vCamAxis));

        if (Input.GetKeyDown(iS.switchShoulders))
            camRig.SwitchShoulders();
    }

    private void UpdateRotation()
    {
        float rotation = (inputType == InputType.PS4Controller ? -Input.GetAxisRaw(iS.hCamAxis) :
            Input.GetAxisRaw(iS.hCamAxis)) * camRig.cameraSettings.camXSens;

        transform.Rotate(0f, rotation, 0f);
    }

    #endregion


    private void UpdateAMR()
    {
        UpdateAMRCharges();

        if (Input.GetKeyDown(iS.jump)) Jump();
        if (Input.GetKeyDown(iS.dash)) Dash();

    }

    private void UpdateWeapon()
    {
        ProjectileTypes oldType = sB.GetSelectedWeapon();
        if (Input.GetKey(iS.shoot))
        {
            sB.Shoot();
        }

        if (inputType == InputType.Keyboard1)
        {
            if (Input.GetAxis(iS.typeScrollAxis) > 0f) sB.SelectWeapon(-1);
            else if (Input.GetAxis(iS.typeScrollAxis) < 0f) sB.SelectWeapon(1);
        }
        else
        {
            if (Input.GetKeyDown(iS.previousType)) sB.SelectWeapon(-1);
            if (Input.GetKeyDown(iS.nextType)) sB.SelectWeapon(1);
        }

        if (inputType != InputType.PS4Controller)
        {
            if (Input.GetKeyDown(iS.switchToRock)) sB.SelectWeapon((uint)1);
            if (Input.GetKeyDown(iS.switchToPaper)) sB.SelectWeapon((uint)2);
            if (Input.GetKeyDown(iS.switchToScissors)) sB.SelectWeapon((uint)3);
        }

        ProjectileTypes newType = sB.GetSelectedWeapon();

        if (oldType != newType) SetTypeIndicator();
    }

    #endregion

    private void ToggleCursor()
    {
        switch (Cursor.lockState)
        {
            case CursorLockMode.None:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorLockMode.Locked:
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    //Remove Method from Class for VerticalSlice
    private void SetTypeIndicator()
    {
        ProjectileTypes pType = sB.GetSelectedWeapon();

        switch (pType)
        {
            case ProjectileTypes.ROCK:
                rockIndicator.SetActive(true);
                paperIndicator.SetActive(false);
                scissorsIndicator.SetActive(false);
                indicatorMaterial.color = new Color(0, 255, 0, 0.1f);
                break;

            case ProjectileTypes.PAPER:
                rockIndicator.SetActive(false);
                paperIndicator.SetActive(true);
                scissorsIndicator.SetActive(false);
                indicatorMaterial.color = new Color(0, 0, 255, 0.1f);
                break;

            case ProjectileTypes.SCISSORS:
                rockIndicator.SetActive(false);
                paperIndicator.SetActive(false);
                scissorsIndicator.SetActive(true);
                indicatorMaterial.color = new Color(255, 0, 0, 0.1f);
                break;

            default:
                break;
        }
    }

}
