using UnityEngine;

namespace rockpaper_djd
{
    [CreateAssetMenu(fileName = "GameModeData",
    menuName = "Data/GameMode/Game Mode Data", order = 0)]
    public class GameModeData : ScriptableObject
    {
        [Header("-ID-")]
        [Tooltip("Name for this Game Mode")]
        public string gameModeName;

        [Tooltip("Description for this Game Mode"), Multiline(10)]
        public string gameModeDescription;


        [Header("-Win Conditions-")]
        [Tooltip("Set Time Limit (MINUTES) for this Game Mode (if Game Mode is Round Based, it refers to Time Limit for each round)")]
        public float timeLimit;

        [Tooltip("Set Score Limit for this Game Mode")]
        public int scoreLimit;


        [Header("-RoundBased-")]
        [Tooltip("Set it Game Mode has rounds (if true, timeLimit refers to how long each round takes)")]
        public bool roundBased;

        [Tooltip("Set number of rounds, if the Game Mode is Round Based")]
        public int numberOfRounds;


        [Header("-ZoneBased-")]
        [Tooltip("Set it Game Mode has zones")]
        public bool zoneBased;

        [Tooltip("Points earned for each second in zone for this Game Mode")]
        public int pointsPerScondInZone;

        [Tooltip("Set interval between zone changes (SECONDS)")]
        public float zoneChangeInterval;


        [Header("-Rules-")]
        [Tooltip("Set how many seconds the pre-match/pre-round timer has (SECONDS)")]
        public int preMatchRoundTimerDuration;

        [Tooltip("Points earned for kill for this Game Mode")]
        public int pointsPerKill;

        [Tooltip("Set if both players are returned to their spawn after every kill")]
        public bool resetBothPlayers = false;

        [Tooltip("Set if Trinity Gauntlet starts incomplete")]
        public bool gauntletStartsIncomplete = false;

        [Tooltip("Enable/Disable Ammo Pickups for this Game Mode")]
        public bool ammoPickups = true;

        [Tooltip("Enable/Disable Health Pickups for this Game Mode")]
        public bool healthPickups = true;

        [Tooltip("Enable/Disable Respawn Immunity for this Game Mode")]
        public bool respawnImmunity = true;

        [Tooltip("Enable/Disable AMR for this Game Mode")]
        public bool AMRAuthorized = true;
    }
}