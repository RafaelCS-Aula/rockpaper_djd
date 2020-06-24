using UnityEngine;

namespace rockpaper_djd
{
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

        [HideInInspector] public int points;
        [HideInInspector] public float kills;
        [HideInInspector] public float deaths;

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