using UnityEngine;

public class PlayerInput : CharacterMovement
{
    [SerializeField] private InputType inputType;

    private InputSettings iS;
    private ShooterBehaviour sB;
    private CameraRig camRig;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        iS = gameObject.AddComponent<InputSettings>();
        iS.SetInputType(inputType);

        camRig = GetComponentInChildren<CameraRig>();
        sB = GetComponent<ShooterBehaviour>();
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
