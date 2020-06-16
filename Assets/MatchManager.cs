using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public class MatchManager : MonoBehaviour
    {
        private GameModeManager gmManager;


        bool gameFinished;

        private int team1Points;
        private int team2Points;

        private float matchClock;

        private void Start()
        {
            gmManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();

            team1Points = 0;
            team2Points = 0;
            matchClock = 0;

        }

        private void Update()
        {
            gameFinished = CheckForWinner();

            if (!gameFinished)
            {

            }


        }


        private bool CheckForWinner()
        {
            if (team1Points >= gmManager.scoreLimit ||
                team2Points >= gmManager.scoreLimit) return true;

            if (matchClock >= gmManager.timeLimit) return true;

            return false;
        }
    }
}