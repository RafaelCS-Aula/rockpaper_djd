using UnityEngine;
using RPS_DJDIII.Assets.Scripts.Enums;

namespace RPS_DJDIII.Assets.Scripts.DataScriptables.ObjectsData
{
    [CreateAssetMenu(fileName = "ManaPickupData",
    menuName = "Data/Pickups/Mana Pickup Data", order = 0)]
    public class ManaPickupData : ScriptableObject
    {
        [Header("Contents")]
        public ProjectileTypes manaGiven;
        public int amount;

        public bool isTemporary;
        public float lifeTime;

        [Header("Shooter Relations")]
        [Tooltip("This pickup gives the shooter the weapon of its Mana type")]
        public bool givesWeapon;

        [Tooltip("This pickup requires it's weapon type to be present in order to be picked up")]
        public bool needsWeapon;

    }
}