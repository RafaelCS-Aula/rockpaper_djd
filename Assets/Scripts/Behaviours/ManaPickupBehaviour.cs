using UnityEngine;
using RPS_DJDIII.Assets.Scripts.Enums;
using RPS_DJDIII.Assets.Scripts.Interfaces;
using RPS_DJDIII.Assets.Scripts.DataScriptables.ObjectsData;
using RPS_DJDIII.Assets.Scripts.Behaviours.CharacterBehaviours;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Rigidbody))]

    /// <summary>
    /// Ammo pickup
    /// </summary>
    public class ManaPickupBehaviour : MonoBehaviour, IDataUser<ManaPickupData>
    {

        [SerializeField] ManaPickupData _dataHolder;
        public ManaPickupData DataHolder
        {
            get => _dataHolder;
            set => value = _dataHolder;
        }

        [Header("---- Data Variables ----")]
        private ProjectileTypes _dManaGiven;
        private int _dAmount;
        private bool _dIsTemporary;
        private float _dLifetime;
        private bool _dGivesWeapon;
        private bool _dNeedsWeapon;

        [Header("---- Pickup Options ------")]
        private float currentTime;
        private SphereCollider pickupCollidier;
        [SerializeField] private float pickupRadius = 0.7f;


        // Start is called before the first frame update
        void Awake()
        {
            pickupCollidier = gameObject.AddComponent<SphereCollider>();
            pickupCollidier.isTrigger = true;
            pickupCollidier.radius = pickupRadius;
            GetData();
            Rigidbody rb = GetComponent<Rigidbody>();
            //rb.useGravity = true;   
            currentTime = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_dIsTemporary)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= _dLifetime)
                    Destroy(gameObject);
            }
        }

        public void GetData()
        {
            _dManaGiven = DataHolder.manaGiven;
            _dAmount = DataHolder.amount;
            _dIsTemporary = DataHolder.isTemporary;
            _dLifetime = DataHolder.lifeTime;
            _dGivesWeapon = DataHolder.givesWeapon;
            _dNeedsWeapon = DataHolder.needsWeapon;
        }

        /// <summary>
        /// When touched by a shooter, increase their ammo and then de-spawn
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {

            ShooterBehaviour ammoReceiver =
                other.gameObject.GetComponent<ShooterBehaviour>();
            bool consumed;

            if (ammoReceiver != null)
            {
                consumed = ammoReceiver.AddMana(_dAmount, _dManaGiven);
                if (consumed)
                    Destroy(gameObject);

                print("ADDED" + _dManaGiven);
            }

        }

        private void OnDrawGizmos()
        {
            switch (_dataHolder.manaGiven)
            {
                case ProjectileTypes.PAPER:
                    Gizmos.color = Color.green;
                    break;
                case ProjectileTypes.ROCK:
                    Gizmos.color = Color.red;
                    break;
                case ProjectileTypes.SCISSORS:
                    Gizmos.color = Color.blue;
                    break;
                case ProjectileTypes.DEFAULT:
                    break;
                default:
                    Gizmos.color = Color.white;
                    break;
            }
            //Gizmos.DrawRay(transform.position, transform.forward * 10);
            //Gizmos.DrawSphere(transform.position, 0.4f);
            Gizmos.DrawWireSphere(transform.position, pickupRadius);


        }
    }
}