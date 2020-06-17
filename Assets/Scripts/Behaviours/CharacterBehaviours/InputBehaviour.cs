﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public class InputBehaviour : MonoBehaviour, IDataUser<InputData>
    {
        #region Data Handling

        [SerializeField] private InputData _dataFile;

        public InputData DataHolder
        {
            get => _dataFile;
            set => value = _dataFile;
        }

        public string inputName;

        public string hMovAxis;
        public string vMovAxis;

        public KeyCode jump;
        public KeyCode dash;


        public string hCamAxis;
        public string vCamAxis;
        public KeyCode switchShoulders;


        public KeyCode shoot;

        public KeyCode previousType;
        public KeyCode nextType;
        public string typeScrollAxis;

        public KeyCode switchToRock;
        public KeyCode switchToPaper;
        public KeyCode switchToScissors;


        public void GetData()
        {
            inputName = DataHolder.inputName;

            hMovAxis = DataHolder.hMovAxis;
            vMovAxis = DataHolder.vMovAxis;

            jump = DataHolder.jump;
            dash = DataHolder.dash;


            hCamAxis = DataHolder.hCamAxis;
            vCamAxis = DataHolder.vCamAxis;
            switchShoulders = DataHolder.switchShoulders;


            shoot = DataHolder.shoot;

            if (DataHolder.previousType != KeyCode.None) previousType = DataHolder.previousType;
            if (DataHolder.nextType != KeyCode.None) nextType = DataHolder.nextType;
            if (DataHolder.typeScrollAxis != "") typeScrollAxis = DataHolder.typeScrollAxis;

            if (DataHolder.switchToRock != KeyCode.None) switchToRock = DataHolder.switchToRock;
            if (DataHolder.switchToPaper != KeyCode.None) switchToPaper = DataHolder.switchToPaper;
            if (DataHolder.switchToScissors != KeyCode.None) switchToScissors = DataHolder.switchToScissors;
        }

        #endregion


        [HideInInspector] public MovementBehaviour mB;
        [HideInInspector] public ShooterBehaviour sB;

        private CameraBehaviour camB;

        #region Helping Vars for other Classes

        [HideInInspector] public ProjectileTypes oldType;
        [HideInInspector] public ProjectileTypes newType;


        #endregion


        void Awake()
        {
            if (DataHolder == null)
            {
                Debug.LogError("Object doesn't have a Data Scriptable Object assigned.");
                throw new UnityException();
            }

            else GetData();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
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
            mB.strafeAxis = Input.GetAxisRaw(hMovAxis);

            mB.forwardAxis = Input.GetAxisRaw(vMovAxis);
        }

        private void UpdateCamera()
        {
            camB.RotateCamera(-Input.GetAxisRaw(vCamAxis));

            if (Input.GetKeyDown(switchShoulders))
                camB.SwitchShoulders();
        }

        private void UpdateRotation()
        {
            float rotation = (inputName == "PS4Controller" ? -Input.GetAxisRaw(hCamAxis) :
                Input.GetAxisRaw(hCamAxis)) * camB.camXSens;

            transform.Rotate(0f, rotation, 0f);
        }

        #endregion


        private void UpdateAMR()
        {
            mB.UpdateAMRCharges();

            if (Input.GetKeyDown(jump)) mB.Jump();
            if (Input.GetKeyDown(dash)) mB.Dash();
        }

        private void UpdateWeapon()
        {
            oldType = sB.GetSelectedWeapon();

            if (Input.GetKey(shoot))
            {
                sB.Shoot();
            }

            if (typeScrollAxis != "")
            {
                if (Input.GetAxis(typeScrollAxis) > 0f) sB.SelectWeapon(-1);
                else if (Input.GetAxis(typeScrollAxis) < 0f) sB.SelectWeapon(1);
            }
            else
            {
                if (Input.GetKeyDown(previousType)) sB.SelectWeapon(-1);
                if (Input.GetKeyDown(nextType)) sB.SelectWeapon(1);
            }

            if (switchToRock != KeyCode.None)
            {
                if (Input.GetKeyDown(switchToRock)) sB.SelectWeapon((uint)1);
                if (Input.GetKeyDown(switchToPaper)) sB.SelectWeapon((uint)2);
                if (Input.GetKeyDown(switchToScissors)) sB.SelectWeapon((uint)3);
            }

            newType = sB.GetSelectedWeapon();
        }

        #endregion
    }
}