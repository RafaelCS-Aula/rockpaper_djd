using UnityEngine;

namespace rockpaper_djd
{
    public class GameModeManager : MonoBehaviour
    {
        #region Vars
        public GameModeData[] gameModes = new GameModeData[3];

        public string gameModeName;

        [Multiline(10)]
        public string gameModeDescription;


        public float timeLimit;
        public int scoreLimit;


        public bool roundBased;
        public int numberOfRounds;


        public bool zoneBased;
        public int pointsPerScondInZone;
        public float zoneChangeInterval;


        public int preMatchRoundTimerDuration;

        public int pointsPerKill;

        public bool resetBothPlayers;

        public bool gauntletStartsIncomplete;

        public bool ammoPickups;

        public bool healthPickups;

        public bool respawnImmunity;

        public bool AMRAuthorized;
        #endregion

        [HideInInspector] public string p1Name;
        [HideInInspector] public string p2Name;


        public void LoadGameModeData(GameModeData gameModeData)
        {
            gameModeName = gameModeData.gameModeName;
            gameModeDescription = gameModeData.gameModeDescription;
            timeLimit = gameModeData.timeLimit;
            scoreLimit = gameModeData.scoreLimit;
            roundBased = gameModeData.roundBased;
            numberOfRounds = gameModeData.numberOfRounds;
            zoneBased = gameModeData.zoneBased;
            pointsPerScondInZone = gameModeData.pointsPerScondInZone;
            zoneChangeInterval = gameModeData.zoneChangeInterval;
            preMatchRoundTimerDuration = gameModeData.preMatchRoundTimerDuration;
            pointsPerKill = gameModeData.pointsPerKill;
            resetBothPlayers = gameModeData.resetBothPlayers;
            gauntletStartsIncomplete = gameModeData.gauntletStartsIncomplete;
            ammoPickups = gameModeData.ammoPickups;
            healthPickups = gameModeData.healthPickups;
            respawnImmunity = gameModeData.respawnImmunity;
            AMRAuthorized = gameModeData.AMRAuthorized;
        }
    }
}