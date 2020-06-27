using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.DataScriptables.CharacterData
{
    /// <summary>
    /// Stores all static info about an object capable of shooting other objects
    /// </summary>
    [CreateAssetMenu(fileName = "Shooter",
    menuName = "Data/Arena/Shooter Data", order = 0)]
    public class ShooterData : ScriptableObject
    {
        [Header("Specs")]
        public float FireRate;

        [Header("Mana")]
        public bool StartWithScissor = true;
        public int StartingScissorMana;
        public int MaxScissorMana;

        public bool StartWithRock = true;
        public int StartingRockMana;
        public int MaxRockMana;

        public bool StartWithPaper = true;
        public int StartingPaperMana;
        public int MaxPaperMana;
    }
}