using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public class MatchManager : MonoBehaviour
    {
        [HideInInspector] public GameModeManager gmManager;


        [HideInInspector] public bool gameFinished;

        public CharacterHandler player1;
        public CharacterHandler player2;


        [SerializeField] private GameObject playersGroup;



        [HideInInspector] public float matchTimer;

        [HideInInspector] public string winner;

        [HideInInspector] public int currentRound = 1;

        [HideInInspector] public int preMatchTimer;
        [HideInInspector] public bool isCountingDown = false;

        private void Start()
        {
            // Find GameModeManager component on the scene object name "GameModeManager"
            gmManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();

            // Set match timer based on the game mode time limit
            matchTimer = gmManager.timeLimit * 60;


            // Disable each player immunity system if the game mode doesn't allow it
            player1.hB.immunityEnabled = gmManager.respawnImmunity;
            player2.hB.immunityEnabled = gmManager.respawnImmunity;

            player1.mB.AMRAuthorized = gmManager.AMRAuthorized;
            player2.mB.AMRAuthorized = gmManager.AMRAuthorized;

            StartCoroutine(PreMatchCountdown());
        }

        private void Update()
        {
            // Determine whether the game is still in progress or not
            gameFinished = CheckForWin();

            // Check if game is still in progress
            if (!gameFinished)
            {
                if (!isCountingDown)
                {
                    UpdateMatchClock();
                    if (!gmManager.roundBased)CheckForKill();
                }
                if (gmManager.roundBased) CheckForRoundEnd();
            }
            else GameOver();

            if (Input.GetKeyDown(KeyCode.P)) NextRound();
        }

        private void UpdateMatchClock() { matchTimer -= Time.deltaTime; }

        private void CheckForKill()
        {
            if (player1.hB._currentHp <= 0)
            {
                player1.hB._currentHp = player1.hB._maxHp;
                player2.points += gmManager.pointsPerKill;
                player1.hB.ResetPosition();
                if (gmManager.resetBothPlayers) player2.hB.ResetPosition();
            }
            if (player2.hB._currentHp <= 0)
            {
                player2.hB._currentHp = player2.hB._maxHp;
                player1.points += gmManager.pointsPerKill;
                player2.hB.ResetPosition();
                if (gmManager.resetBothPlayers) player1.hB.ResetPosition();
            }
        }




        private void NextRound()
        {
            player1.hB._currentHp = player1.hB._maxHp;
            player2.hB._currentHp = player2.hB._maxHp;
            player1.hB.ResetPosition();
            player2.hB.ResetPosition();
            player1.sB.FillMana();
            player2.sB.FillMana();
            player1.mB.ResetAMRCHarges();
            player2.mB.ResetAMRCHarges();
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
            }
            if (player2.hB._currentHp <= 0)
            {
                player1.points += gmManager.pointsPerKill;
                NextRound();
            }
        }










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

            playersGroup.SetActive(false);
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

    }
}