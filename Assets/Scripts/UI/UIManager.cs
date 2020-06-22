using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace rockpaper_djd
{
    public class UIManager : MonoBehaviour
    {
        #region Panels
        [Header("-PANELS-")]
        [SerializeField] private GameObject PreMatchRoundPanel;
        [SerializeField] private GameObject GameOverPanel;
        #endregion

        #region Displays
        [Header("-DISPLAYS-")]
        [SerializeField] private Image p1Crosshair;
        [SerializeField] private Image p2Crosshair;

        [SerializeField] private TextMeshProUGUI p1AmmoDisplay;
        [SerializeField] private TextMeshProUGUI p2AmmoDisplay;

        [SerializeField] private GameObject rockIndP1;
        [SerializeField] private GameObject paperIndP1;
        [SerializeField] private GameObject scissorsIndP1;

        [SerializeField] private GameObject rockIndP2;
        [SerializeField] private GameObject paperIndP2;
        [SerializeField] private GameObject scissorsIndP2;


        [SerializeField] private TextMeshProUGUI clock;
        [SerializeField] private TextMeshProUGUI winner;
        [SerializeField] private TextMeshProUGUI team1Points;
        [SerializeField] private TextMeshProUGUI team2Points;

        [SerializeField] private TextMeshProUGUI round;
        [SerializeField] private TextMeshProUGUI preMatchRoundTimer;
        #endregion

        #region Aim Sprites
        [Header("-AIM SPRITES-")]
        [SerializeField] private Sprite aim00;
        [SerializeField] private Sprite aim01;
        [SerializeField] private Sprite aim02;
        [SerializeField] private Sprite aim10;
        [SerializeField] private Sprite aim11;
        [SerializeField] private Sprite aim12;
        [SerializeField] private Sprite aim20;
        [SerializeField] private Sprite aim21;
        [SerializeField] private Sprite aim22;

        #endregion

        #region Match Vars

        [SerializeField] private MatchManager matchManager;


        #endregion

        private void Awake()
        {
            Destroy(matchManager.player2.gameObject.GetComponentInChildren<AudioListener>());
        }
        private void Update()
        {
            if (!matchManager.gameFinished)
            {
                #region Crosshair Updates
                UpdateCrosshair(matchManager.player1, p1Crosshair);
                UpdateCrosshair(matchManager.player2, p2Crosshair);
                #endregion
                #region AmmoDisplay Updates
                UpdateAmmoDisplay(matchManager.player1, p1AmmoDisplay);
                UpdateAmmoDisplay(matchManager.player2, p2AmmoDisplay);
                #endregion
                #region Indicator Updates
                if (matchManager.player1.iB.oldType != matchManager.player1.iB.newType) UpdateIndicator(matchManager.player1,
                    rockIndP1, paperIndP1, scissorsIndP1);

                if (matchManager.player2.iB.oldType != matchManager.player2.iB.newType) UpdateIndicator(matchManager.player2,
                    rockIndP2, paperIndP2, scissorsIndP2);
                #endregion
                UpdateClockDisplay();
                UpdatePointsDisplay();
                UpdatePreMatchTimer();
                if (matchManager.gmManager.roundBased) UpdateRoundDisplay();
            }


            if (matchManager.gameFinished) GameOverDisplay();

        }

        private void UpdateCrosshair(CharacterHandler player, Image crosshair)
        {
            Sprite newSprite = crosshair.sprite;

            if (player.mB.doubleJumpCharges == 0 && player.mB.dashCharges == 0) newSprite = aim00;
            else if (player.mB.doubleJumpCharges == 0 && player.mB.dashCharges == 1) newSprite = aim01;
            else if (player.mB.doubleJumpCharges == 0 && player.mB.dashCharges == 2) newSprite = aim02;
            else if (player.mB.doubleJumpCharges == 1 && player.mB.dashCharges == 0) newSprite = aim10;
            else if (player.mB.doubleJumpCharges == 1 && player.mB.dashCharges == 1) newSprite = aim11;
            else if (player.mB.doubleJumpCharges == 1 && player.mB.dashCharges == 2) newSprite = aim12;
            else if (player.mB.doubleJumpCharges == 2 && player.mB.dashCharges == 0) newSprite = aim20;
            else if (player.mB.doubleJumpCharges == 2 && player.mB.dashCharges == 1) newSprite = aim21;
            else if (player.mB.doubleJumpCharges == 2 && player.mB.dashCharges == 2) newSprite = aim22;


            crosshair.overrideSprite = newSprite;
        }

        private void UpdateAmmoDisplay(CharacterHandler player, TextMeshProUGUI ammoDisplay)
        {
            (float maxRock, float currentRock) = player.sB.GetMana(ProjectileTypes.ROCK);
            (float maxPaper, float currentPaper) = player.sB.GetMana(ProjectileTypes.PAPER);
            (float maxScissors, float currentScissors) = player.sB.GetMana(ProjectileTypes.SCISSORS);
            ammoDisplay.text = $"Cylinder (Rock) ammo: {currentRock}/{maxRock}\n" +
                $"Sphere (Paper) ammo: {currentPaper}/{maxPaper}\n" +
                $"Cube (Scissors) ammo: {currentScissors}/{maxScissors}\n";
        }

        private void UpdateIndicator(CharacterHandler player, GameObject rockInd,
            GameObject paperInd, GameObject scissorsInd)
        {
            ProjectileTypes pType = player.sB.GetSelectedWeapon();

            switch (pType)
            {
                case ProjectileTypes.ROCK:
                    rockInd.SetActive(true);
                    paperInd.SetActive(false);
                    scissorsInd.SetActive(false);
                    break;

                case ProjectileTypes.PAPER:
                    rockInd.SetActive(false);
                    paperInd.SetActive(true);
                    scissorsInd.SetActive(false);
                    break;

                case ProjectileTypes.SCISSORS:
                    rockInd.SetActive(false);
                    paperInd.SetActive(false);
                    scissorsInd.SetActive(true);
                    break;

                default:
                    break;
            }
        }

        private void UpdateClockDisplay()
        {
            string minutes = Mathf.Floor(matchManager.matchTimer / 60).ToString("00");
            string seconds = (matchManager.matchTimer % 60).ToString("00");

            // Check if the clock displays "60" seconds (This is done to correct cases like, i.e. the clock displays 3:60 istead of 4:00)
            if (seconds == "60")
            {
                // Display "00" seconds instead
                seconds = "00";
                // Add 1 minute to the display
                minutes = Mathf.Floor((matchManager.matchTimer / 60) + 1).ToString("00");
            }
            // Correct the wrong display when the timer reaches 0
            if (minutes == "-01") minutes = "00";

            // Change clock text to display the current minutes and seconds
            clock.text = minutes + ":" + seconds;
        }

        private void UpdatePointsDisplay()
        {
            string points1;
            string points2;

            points1 = matchManager.player1.points.ToString("000") + " -";
            points2 = "- " + matchManager.player2.points.ToString("000");

            if(team1Points.text != points1) team1Points.text = points1;
            if(team2Points.text != points2) team2Points.text = points2;
        }

        private void UpdatePreMatchTimer()
        {
            if (matchManager.isCountingDown)
            {
                if (!PreMatchRoundPanel.activeSelf) PreMatchRoundPanel.SetActive(true);

                preMatchRoundTimer.text = matchManager.preMatchTimer.ToString();
            }

            else
            {
                if (PreMatchRoundPanel.activeSelf) PreMatchRoundPanel.SetActive(false);
            }
        }

        private void UpdateRoundDisplay()
        {
            if (round.text != matchManager.currentRound.ToString())
                round.text = "Round " + matchManager.currentRound.ToString("00");
        }

        private void GameOverDisplay()
        {
            GameOverPanel.SetActive(true);
            if (matchManager.winner != "")
                winner.text = matchManager.winner + " wins!!";
            else winner.text = "it's a draw!!";
        }
    }
}