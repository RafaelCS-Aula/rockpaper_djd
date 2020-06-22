using UnityEngine;

namespace rockpaper_djd
{
    public class GameModeManager : MonoBehaviour
    {
        public GameModeData[] gameModes = new GameModeData[3];

        public string gameModeName;

        [Multiline(10)]
        public string gameModeDescription;


        public float timeLimit;
        public int scoreLimit;


        public int pointsPerKill;

        public int pointsPerScondInZone;

        public int preMatchRoundTimerDuration;

        public bool roundBased;

        public int numberOfRounds;


        public bool resetBothPlayers;

        public bool gauntletStartsIncomplete;

        public bool[] availableAmmoTypes = new bool[3];

        public bool ammoPickups;

        public bool healthPickups;

        public bool respawnImmunity;

        public bool AMRAuthorized;

        public bool smokeScreen;


        public void LoadGameModeData(GameModeData gameModeData)
        {
            gameModeName = gameModeData.gameModeName;
            gameModeDescription = gameModeData.gameModeDescription;
            timeLimit = gameModeData.timeLimit;
            scoreLimit = gameModeData.scoreLimit;
            pointsPerKill = gameModeData.pointsPerKill;
            pointsPerScondInZone = gameModeData.pointsPerScondInZone;
            preMatchRoundTimerDuration = gameModeData.preMatchRoundTimerDuration;
            roundBased = gameModeData.roundBased;
            numberOfRounds = gameModeData.numberOfRounds;
            resetBothPlayers = gameModeData.resetBothPlayers;
            gauntletStartsIncomplete = gameModeData.gauntletStartsIncomplete;
            availableAmmoTypes = gameModeData.availableAmmoTypes;
            ammoPickups = gameModeData.ammoPickups;
            healthPickups = gameModeData.healthPickups;
            respawnImmunity = gameModeData.respawnImmunity;
            AMRAuthorized = gameModeData.AMRAuthorized;
            smokeScreen = gameModeData.smokeScreen;
        }
    }
}