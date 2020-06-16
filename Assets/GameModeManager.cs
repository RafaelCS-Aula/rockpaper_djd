using UnityEngine;

namespace rockpaper_djd
{
    public class GameModeManager : MonoBehaviour
    {
        #region Data Handling

        public GameModeData[] gameModes = new GameModeData[3];

        public string gameModeName;

        [Multiline(10)]
        public string gameModeDescription;



        public float timeLimit;
        public int scoreLimit;



        public int pointsPerKill;
        public int pointsPerScondInZone;

        public bool resetBothPlayers = false;

        public bool arenaChanges;

        public bool[] availableAmmoTypes = new bool[3];

        public bool ammoPickups;

        public bool healthPickups;

        public bool respawnImmunity;

        public bool AMR;

        public bool smokeScreen;

        #endregion

        public void LoadGameModeData(GameModeData gameModeData)
        {
            gameModeName = gameModeData.gameModeName;
            gameModeDescription = gameModeData.gameModeDescription;
            timeLimit = gameModeData.timeLimit;
            scoreLimit = gameModeData.scoreLimit;
            pointsPerKill = gameModeData.pointsPerKill;
            pointsPerScondInZone = gameModeData.pointsPerScondInZone;
            resetBothPlayers = gameModeData.resetBothPlayers;
            arenaChanges = gameModeData.arenaChanges;
            availableAmmoTypes = gameModeData.availableAmmoTypes;
            ammoPickups = gameModeData.ammoPickups;
            healthPickups = gameModeData.healthPickups;
            respawnImmunity = gameModeData.respawnImmunity;
            AMR = gameModeData.AMR;
            smokeScreen = gameModeData.smokeScreen;
        }
    }
}