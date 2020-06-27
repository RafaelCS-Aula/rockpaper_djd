using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPS_DJDIII.Assets.Scripts.Enums;
using RPS_DJDIII.Assets.Scripts.Behaviours;
using RPS_DJDIII.Assets.Scripts.ScoreKeeping;
using RPS_DJDIII.Assets.Scripts.ArenaGeneration;


namespace RPS_DJDIII.Assets.Scripts.GameManagers
{
    public class MatchManager : MonoBehaviour
    {
        private List<ArenaPiece> _playArena;

        [HideInInspector] public GameModeManager gmManager;
        [HideInInspector] private HallOfFameManager hofManager;
        [HideInInspector] private GenerationManager genManager;

        private HallOfFameManager.SaveData saveData;


        [HideInInspector] public bool gameFinished;

        public CharacterHandler player1;
        public CharacterHandler player2;


        [SerializeField] private GameObject playersGroup;



        [HideInInspector] public float matchTimer;

        [HideInInspector] public string winner;

        [HideInInspector] public int currentRound = 1;

        [HideInInspector] public int preMatchTimer;
        [HideInInspector] public bool isCountingDown = false;


        [HideInInspector] public List<ZoneBehaviour> zonesList;
        [HideInInspector] public int activeZone;

        [HideInInspector] public float zoneChangeTimer;
        private float zonePointsTimer;
        private int currentSecond;
        private int lastSecond;

        private bool doneAdding = false;


        private void Start()
        {
            // Find GameModeManager component on the scene object named "GameModeManager"
            gmManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();

            // Find HallOfFameManager component on the scene object named "HallOfFameManager"
            hofManager = GameObject.Find("HallOfFameManager").GetComponent<HallOfFameManager>();

            // Find GenerationManager component on the scene object named "GenerationManager"
            genManager = GameObject.Find("GenerationManager").GetComponent<GenerationManager>();

            #region Initialize Vars
            player1.characterName = gmManager.p1Name;
            player2.characterName = gmManager.p2Name;

            // Set match timer based on the game mode time limit
            matchTimer = gmManager.timeLimit * 60;

            // Disable each player immunity system if the game mode doesn't allow it
            player1.hB.immunityEnabled = gmManager.respawnImmunity;
            player2.hB.immunityEnabled = gmManager.respawnImmunity;

            player1.mB.AMRAuthorized = gmManager.AMRAuthorized;
            player2.mB.AMRAuthorized = gmManager.AMRAuthorized;
            #endregion



            _playArena = new List<ArenaPiece>();
            _playArena = genManager.Create();



            // Get the position further away from the center piece, and then the
            // position furthest from that one.
            #region GetPlayerSpawnPositions

            // this list will hold the distances we want compared and the index
            // of the piece we measured, for easy access later.
            List<(float mag, int index)> distAndIndex =
             new List<(float mag, int index)>();
            
            // Get the central piece´s position
            Vector3 centerPiece = _playArena[0].transform.position;
            Vector3 firstPlayerSpawn = new Vector3();
            Vector3 secondPlayerSpawn = new Vector3();

            // All the piece3's distances from the center:
            for(int i = 1; i < _playArena.Count; i++)
            {
                (float distance, int dex) d;
                d.distance = 
                (_playArena[i].transform.position - centerPiece).magnitude;
                d.dex = i;

                distAndIndex.Add(d);
            }

            // sort the list by magnitue
            distAndIndex.Sort((t1, t2) => t1.mag.CompareTo(t2.mag));

            firstPlayerSpawn = 
                _playArena[distAndIndex[0].index].transform.position;
            

            //Now get second player spawn, the furthest piece from the
            // first player's spawn

            // reset the list to use it for measuring with firstPlayerSpawn
            distAndIndex =
             new List<(float mag, int index)>();

            for(int i = 1; i < _playArena.Count; i++)
            {
                (float distance, int dex) d;
                d.distance = 
                (_playArena[i].transform.position - firstPlayerSpawn).magnitude;
                d.dex = i;

                distAndIndex.Add(d);
            }

            // sort the list by magnitue
            distAndIndex.Sort((t1, t2) => t2.mag.CompareTo(t1.mag));

            secondPlayerSpawn = 
                _playArena[distAndIndex[0].index].transform.position;

            // Now, depending on you want to spawn the players, spawn them at
            // the positions firstPlayerSpawn and secondPlayerSpawn have stored.
            player1.mB.SetSpawnPosition(firstPlayerSpawn);
            player2.mB.SetSpawnPosition(secondPlayerSpawn);

            player1.mB.SetSpawnRotation(player2.transform);
            player2.mB.SetSpawnRotation(player1.transform);

            player1.mB.ResetPosition();
            player2.mB.ResetPosition();

            #endregion


            InitArena();

            zonesList.AddRange(_playArena[0].GetComponentsInChildren<ZoneBehaviour>());

            foreach (ZoneBehaviour z in zonesList)
            {
                z.gameObject.SetActive(false);
            }

            if (gmManager.zoneBased)
            {
                int zone = Random.Range(0, zonesList.Count);
                zonesList[zone].gameObject.SetActive(true);
                activeZone = zone;
                zoneChangeTimer = gmManager.zoneChangeInterval; 
            }

            Cursor.lockState = CursorLockMode.Locked;

            StartCoroutine(PreMatchCountdown());
        }

        private void Update()
        {
            // Determine whether the game is still in progress or not
            gameFinished = CheckForWin();

            // Run if game is still in progress
            if (!gameFinished)
            {
                // Run if the pre-match/pre-round clock is not counting down
                if (!isCountingDown)
                {
                    // Update all match timers
                    UpdateTimers();

                    // Check if any player is killed
                    if (!gmManager.roundBased) CheckForKill();

                    #region ZoneBased Specifics
                    // Run only if the Game Mode is Zone Based
                    if (gmManager.zoneBased)
                    {
                        // Update the current zone
                        UpdateZoneChange();

                        // Update the points of the players inside zone
                        UpdateZonePoints();
                    }
                    #endregion
                }
                // If the Game Mode is Round Based, check if the round should end
                if (gmManager.roundBased) CheckForRoundEnd();
            }
            // If the game is not in progress, display game over screen
            else GameOver();
        }

        private void InitArena()
        {
            foreach (ArenaPiece piece in _playArena)
            {
                piece.Initialize();
            }
        }

        private void UpdateTimers()
        {
            matchTimer -= Time.deltaTime;

            if (gmManager.zoneBased)
            {
                zonePointsTimer += Time.deltaTime;
                zoneChangeTimer -= Time.deltaTime;
            }
        }

        private void CheckForKill()
        {
            if (player1.hB._currentHp <= 0)
            {
                player1.hB._currentHp = player1.hB._maxHp;
                player2.points += gmManager.pointsPerKill;
                KillReset(player1);
                if (gmManager.resetBothPlayers) KillReset(player2);

                player1.deaths += 1;
                player2.kills += 1;
            }
            if (player2.hB._currentHp <= 0)
            {
                player2.hB._currentHp = player2.hB._maxHp;
                player1.points += gmManager.pointsPerKill;
                KillReset(player2);
                if (gmManager.resetBothPlayers) KillReset(player1);

                player1.kills += 1;
                player2.deaths += 1;
            }
        }
        #region RoundBased Methods
        private void NextRound()
        {
            RoundReset(player1);
            RoundReset(player2);
            currentRound++;
            matchTimer = gmManager.timeLimit * 60;

            StartCoroutine(PreMatchCountdown());
        }

        private void CheckForRoundEnd()
        {
            if (matchTimer <= 0) NextRound();

            if (player1.hB._currentHp <= 0)
            {
                player2.points += gmManager.pointsPerKill;
                NextRound();
                player1.deaths += 1;
                player2.kills += 1;
            }
            if (player2.hB._currentHp <= 0)
            {
                player1.points += gmManager.pointsPerKill;
                NextRound();
                player1.kills += 1;
                player2.deaths += 1;
            }
        }
        #endregion



        #region ZoneBased Methods
        private void NewZone()
        {
            int newZone = activeZone;
            while (newZone == activeZone)
            {
                newZone = Random.Range(0, zonesList.Count);
            }

            zonesList[activeZone].gameObject.SetActive(false);
            activeZone = newZone;
            zonesList[activeZone].gameObject.SetActive(true);
        }

        private void UpdateZonePoints()
        {
            lastSecond = currentSecond;
            currentSecond = (int)zonePointsTimer;

            if (currentSecond - lastSecond == 1)
            {
                if (zonesList[activeZone].currentOccupant == ZoneOccupants.TEAM1)
                    player1.points += gmManager.pointsPerScondInZone;

                if (zonesList[activeZone].currentOccupant == ZoneOccupants.TEAM2)
                    player2.points += gmManager.pointsPerScondInZone;
            }
        }

        private void UpdateZoneChange()
        {
            if (zoneChangeTimer <= 0)
            {
                NewZone();
                zoneChangeTimer = gmManager.zoneChangeInterval;
            }
        }
        #endregion


        private bool CheckForWin()
        {
            if (player1.points >= gmManager.scoreLimit ||
                player2.points >= gmManager.scoreLimit) return true;

            if (!gmManager.roundBased && matchTimer <= 0) return true;

            if (gmManager.roundBased && currentRound > gmManager.numberOfRounds) return true;

            return false;
        }

        private void GameOver()
        {
            if (player1.points >= gmManager.scoreLimit)
                winner = player1.characterName;

            else if (player2.points >= gmManager.scoreLimit)
                winner = player2.characterName;

            // Load save file
            saveData = hofManager.LoadSaveData();

            // Try to add the two players to the Hall of Fame
            string gameMode = gmManager.gameModeName;

            // Abrevviate King of the Hill name for Hall of Fame display
            if (gameMode == "King of the Hill") gameMode = "KOTH";

            if (!doneAdding)
            {
                hofManager.AddEntry(saveData, player1, gameMode);
                hofManager.AddEntry(saveData, player2, gameMode);
                doneAdding = true;
            }

            playersGroup.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        IEnumerator PreMatchCountdown()
        {
            isCountingDown = true;

            player1.iB.enabled = false;
            player2.iB.enabled = false;
            preMatchTimer = gmManager.preMatchRoundTimerDuration;

            while (preMatchTimer > 0)
            {
                yield return new WaitForSeconds(1);
                preMatchTimer--;
            }
            isCountingDown = false;

            player1.iB.enabled = true;
            player2.iB.enabled = true;
        }

        private void KillReset(CharacterHandler player)
        {
            player.mB.ResetPosition();
            player.hB.StartImmunity();
        }

        private void RoundReset(CharacterHandler player)
        {
            player.mB.ResetPosition();
            player.hB.StartImmunity();
            player.sB.FillMana();
            player.mB.ResetAMRCHarges();
        }
    }
}