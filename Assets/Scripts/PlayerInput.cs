using System;
using UnityEngine;

namespace rockpaper_djd
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private InputType inputType;

        [HideInInspector] public MovementBehaviour mB;
        [HideInInspector] public ShooterBehaviour sB;

        private InputSettings iS;
        private CameraBehaviour camB;

        #region Helping Vars for other Classes

        [HideInInspector] public ProjectileTypes oldType;
        [HideInInspector] public ProjectileTypes newType;


        #endregion


        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
            iS = gameObject.AddComponent<InputSettings>();
            iS.SetInputType(inputType);

            mB = GetComponent<MovementBehaviour>();
            sB = GetComponent<ShooterBehaviour>();

            camB = GetComponentInChildren<CameraBehaviour>();

            mB.audioHandler = GetComponent<PlayerSoundHandler>();
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
            mB.strafeAxis = Input.GetAxisRaw(iS.hMovAxis);

            mB.forwardAxis = Input.GetAxisRaw(iS.vMovAxis);
        }

        private void UpdateCamera()
        {
            camB.RotateCamera(-Input.GetAxisRaw(iS.vCamAxis));

            if (Input.GetKeyDown(iS.switchShoulders))
                camB.SwitchShoulders();
        }

        private void UpdateRotation()
        {
            float rotation = (inputType == InputType.PS4Controller ? -Input.GetAxisRaw(iS.hCamAxis) :
                Input.GetAxisRaw(iS.hCamAxis)) * camB.camXSens;

            transform.Rotate(0f, rotation, 0f);
        }

        #endregion


        private void UpdateAMR()
        {
            mB.UpdateAMRCharges();

            if (Input.GetKeyDown(iS.jump)) mB.Jump();
            if (Input.GetKeyDown(iS.dash)) mB.Dash();

        }

        private void UpdateWeapon()
        {
            oldType = sB.GetSelectedWeapon();

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

            newType = sB.GetSelectedWeapon();
        }

        #endregion
    }
}