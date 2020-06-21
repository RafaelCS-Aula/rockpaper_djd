using System;
using UnityEngine;

namespace rockpaper_djd
{
    public class MatchManager : MonoBehaviour
    {
        private GameModeManager gmManager;


        [HideInInspector] public bool gameFinished;

        public CharacterHandler player1;
        public CharacterHandler player2;


        [SerializeField] private GameObject playersGroup;



        [HideInInspector] public float matchTimer;

        [HideInInspector] public string winner;

        private void Start()
        {
            gmManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();
            matchTimer = gmManager.timeLimit * 60;
        }

        private void Update()
        {
            if (!gameFinished)
            {
                gameFinished = CheckForWinner();
            }


            if (!gameFinished)
            {
                UpdateMatchClock();
                CheckForKill();
            }
            else GameOver();

            if (Input.GetKeyDown(KeyCode.Keypad1)) player1.points++;
            if (Input.GetKeyDown(KeyCode.Keypad2)) player2.points++;
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




        private void GameOver()
        {
            if (player1.points >= gmManager.scoreLimit)
                winner = player1.characterName;
            
            else if (player2.points >= gmManager.scoreLimit)
                winner = player2.characterName;
               
            playersGroup.SetActive(false);
        }

























        private bool CheckForWinner()
        {
            if (player1.points >= gmManager.scoreLimit ||
                player2.points >= gmManager.scoreLimit) return true;

            if (matchTimer <= 0) return true;

            return false;
        }
    }
}