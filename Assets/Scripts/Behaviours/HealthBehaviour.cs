using RPS_DJDIII.Assets.Scripts.Interfaces;
using UnityEngine;
using System.Collections;
using RPS_DJDIII.Assets.Scripts.Behaviours.CharacterBehaviours;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    public class HealthBehaviour : MonoBehaviour, IUseTeams
    {

        public int _maxHp;
        [HideInInspector] public int _currentHp;
        [SerializeField] private int _startingHp;
        [SerializeField] private bool startWithMaxHp = true;

        [SerializeField] private float _damagedCooldown;
        private float _currentCooldown;

        public int teamID { get; set; }


        private CharacterController cC;
        private ShooterBehaviour sB;

        private Vector3 spawnPosition;
        private Quaternion spawnRotation;

        [HideInInspector] public bool immunityEnabled = true;

        private float immunityTimer;
        [SerializeField] private float immunityDuration;



        [SerializeField] private GameObject playerModel;

        // Start is called before the first frame update
        void Awake()
        {
            if (startWithMaxHp)
                _startingHp = _maxHp;

            _currentHp = _startingHp;

            _currentCooldown = _damagedCooldown;

            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
        }

        private void Start()
        {
            cC = GetComponent<CharacterController>();
            sB = GetComponent<ShooterBehaviour>();
        }

        private void Update()
        {
            _currentCooldown += Time.deltaTime;

            _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);


            if (immunityTimer > 0 && immunityEnabled) immunityTimer -= Time.deltaTime;

            FlashPlayer();
        }

        public void InteractFriend(IUseTeams other) { }
        public void InteractEnemy(IUseTeams other)
        {
            if (_currentCooldown >= _damagedCooldown && immunityTimer <= 0)
            {
                print("-1");
                _currentHp--;
                _currentCooldown = 0;
            }
        }

        public void ResetPosition()
        {
            cC.enabled = false;
            transform.localPosition = spawnPosition;
            transform.localRotation = spawnRotation;
            cC.enabled = true;
            if (immunityEnabled) immunityTimer = immunityDuration;
        }

        private void FlashPlayer()
        {
            if (immunityTimer > 0.0f)
            {
                immunityTimer -= Time.deltaTime;

                playerModel.SetActive((Mathf.FloorToInt(immunityTimer * 4.0f) % 2) == 0);

                if (sB.enabled) sB.enabled = false;


            }
            if (immunityTimer <= 0.0f)
            {
                if (!sB.enabled) sB.enabled = true;
                if (!playerModel.activeSelf) playerModel.SetActive(true);
            }
        }
    }
}
