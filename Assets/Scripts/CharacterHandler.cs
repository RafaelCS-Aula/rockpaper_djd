using UnityEngine;
using RPS_DJDIII.Assets.Scripts.Behaviours;
using RPS_DJDIII.Assets.Scripts.Sound;
using RPS_DJDIII.Assets.Scripts.Interfaces;
using RPS_DJDIII.Assets.Scripts.Behaviours.CharacterBehaviours;

namespace RPS_DJDIII.Assets.Scripts
{
    /// <summary>
    /// Class that stores a reference to every CharacterBehaviour component
    /// </summary>
    public class CharacterHandler : MonoBehaviour, ISoundPlayer<PlayerSoundHandler>
    {
        public string characterName;
        #region Player Components
        [HideInInspector] public InputBehaviour iB;
        [HideInInspector] public MovementBehaviour mB;
        [HideInInspector] public ShooterBehaviour sB;
        [HideInInspector] public TeamMemberBehaviour tB;
        [HideInInspector] public HealthBehaviour hB;

        [HideInInspector] public CameraBehaviour cB;
        [HideInInspector] public PlayerSoundHandler audioHandler { get; set; }
        #endregion


        public int points;
        public float kills;
        public float deaths;

        private void Start()
        {
            iB = GetComponent<InputBehaviour>();
            mB = GetComponent<MovementBehaviour>();
            sB = GetComponent<ShooterBehaviour>();
            tB = GetComponent<TeamMemberBehaviour>();
            hB = GetComponent<HealthBehaviour>();

            cB = GetComponentInChildren<CameraBehaviour>();

            audioHandler = GetComponent<PlayerSoundHandler>();

            points = 0;
        }
    }
        
}
