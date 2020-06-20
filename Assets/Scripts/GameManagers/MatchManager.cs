using System;
using UnityEngine;

namespace rockpaper_djd
{
    public class MatchManager : MonoBehaviour
    {
        private GameModeManager gmManager;


        bool gameFinished;

        private int team1Points;
        private int team2Points;

        [HideInInspector] public float matchTimer;

        private void Start()
        {
            gmManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();

            team1Points = 0;
            team2Points = 0;
            matchTimer = gmManager.timeLimit*60;

        }

        private void Update()
        {
            gameFinished = CheckForWinner();


            if (!gameFinished)
            {
                UpdateMatchClock();
            }
        }

        private void UpdateMatchClock() { matchTimer -= Time.deltaTime; }





























            private bool CheckForWinner()
        {
            if (team1Points >= gmManager.scoreLimit ||
                team2Points >= gmManager.scoreLimit) return true;

            if (matchTimer <= 0) return true;

            return false;
        }
    }
}