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
        [Tooltip("Set Time Limit for this Game Mode (MINUTES)")]
        public float timeLimit;

        [Tooltip("Set Score Limit for this Game Mode")]
        public int scoreLimit;


        [Header("-Rules-")]
        [Tooltip("Points earned for kill for this Game Mode")]
        public int pointsPerKill;

        [Tooltip("Points earned for each second in zone for this Game Mode")]
        public int pointsPerScondInZone;

        [Tooltip("Set if both players are returned to their spawn after every kill")]
        public bool resetBothPlayers = false;

        [Tooltip("Set if Trinity Gauntlet starts incomplete")]
        public bool gauntletStartsIncomplete = false;

        [Tooltip("Set Available Ammo Types for this Game Mode")]
        public bool[] availableAmmoTypes = new bool[3] { true, true, true };

        [Tooltip("Enable/Disable Ammo Pickups for this Game Mode")]
        public bool ammoPickups = true;

        [Tooltip("Enable/Disable Health Pickups for this Game Mode")]
        public bool healthPickups = true;

        [Tooltip("Enable/Disable Respawn Immunity for this Game Mode")]
        public bool respawnImmunity = true;

        [Tooltip("Enable/Disable AMR for this Game Mode")]
        public bool AMR = true;

        [Tooltip("Enable/Disable Smoke Screen for this Game Mode")]
        public bool smokeScreen = true;
    }
}