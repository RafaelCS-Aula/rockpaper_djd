using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPS_DJDIII.Assets.Scripts.Enums;
using RPS_DJDIII.Assets.Scripts.GameManagers;

namespace RPS_DJDIII.Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MatchManager matchManager;

        #region Panels
        [Header("-PANELS-")]
        [SerializeField] private GameObject PreMatchRoundPanel;
        [SerializeField] private GameObject GameOverPanel;
        #endregion

        #region Displays
        [Header("-DISPLAYS-")]
        [SerializeField] private TextMeshProUGUI p1Health;
        [SerializeField] private TextMeshProUGUI p2Health;
        
        [SerializeField] private Image p1Abilities;
        [SerializeField] private Image p2Abilities;
        
        [SerializeField] private Image p1SelectedProjectile;
        [SerializeField] private Image p2SelectedProjectile;

        [SerializeField] private TextMeshProUGUI p1Ammo;
        [SerializeField] private TextMeshProUGUI p2Ammo;

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

        [SerializeField] private TextMeshProUGUI zoneTimer;
        [SerializeField] private TextMeshProUGUI zoneOccupant;
        #endregion

        #region Ability Sprites
        [Header("-AIM SPRITES-")]
        [SerializeField] private Sprite ability00;
        [SerializeField] private Sprite ability01;
        [SerializeField] private Sprite ability02;
        [SerializeField] private Sprite ability10;
        [SerializeField] private Sprite ability11;
        [SerializeField] private Sprite ability12;
        [SerializeField] private Sprite ability20;
        [SerializeField] private Sprite ability21;
        [SerializeField] private Sprite ability22;

        #endregion
        
        #region Projectile Sprites
        [Header("-AIM SPRITES-")]
        [SerializeField] private Sprite rock;
        [SerializeField] private Sprite paper;
        [SerializeField] private Sprite scissors;
        #endregion


        private void Awake()
        {
            Destroy(matchManager.player2.gameObject.GetComponentInChildren<AudioListener>());
        }
        private void Update()
        {
            if (!matchManager.gameFinished)
            {
                #region Health Updates
                UpdateHealthDisplay(matchManager.player1, p1Health);
                UpdateHealthDisplay(matchManager.player2, p2Health);
                #endregion
                #region Ability Updates
                UpdateAbilitiesDisplay(matchManager.player1, p1Abilities);
                UpdateAbilitiesDisplay(matchManager.player2, p2Abilities);
                #endregion
                #region AmmoDisplay Updates
                UpdateAmmoDisplay(matchManager.player1, p1Ammo);
                UpdateAmmoDisplay(matchManager.player2, p2Ammo);
                #endregion
                #region Indicator Updates
                if (matchManager.player1.iB.oldType != matchManager.player1.iB.newType) UpdateIndicatorDisplay(matchManager.player1,
                    rockIndP1, paperIndP1, scissorsIndP1, p1SelectedProjectile);

                if (matchManager.player2.iB.oldType != matchManager.player2.iB.newType) UpdateIndicatorDisplay(matchManager.player2,
                    rockIndP2, paperIndP2, scissorsIndP2, p2SelectedProjectile);
                #endregion
                UpdateClockDisplay();
                UpdatePointsDisplay();
                UpdatePreMatchTimer();
                if (matchManager.gmManager.roundBased) UpdateRoundDisplay();
                if (matchManager.gmManager.zoneBased)
                {
                    UpdateZoneTimer();
                    UpdateZoneOccupant();
                }
            }

            if (matchManager.gameFinished) GameOverDisplay();

        }

        private void UpdateHealthDisplay(CharacterHandler player, TextMeshProUGUI health)
        {
            string currentHP = player.hB._currentHp.ToString();
            if (health.text != currentHP) health.text = currentHP;
        }


        private void UpdateAbilitiesDisplay(CharacterHandler player, Image abilities)
        {
            Sprite newSprite = abilities.sprite;
            int djCharges = player.mB.doubleJumpCharges;
            int dashCharges = player.mB.dashCharges;

            if (djCharges == 0 && dashCharges == 0) newSprite = ability00;
            else if (djCharges == 0 && dashCharges == 1) newSprite = ability01;
            else if (djCharges == 0 && dashCharges == 2) newSprite = ability02;
            else if (djCharges == 1 && dashCharges == 0) newSprite = ability10;
            else if (djCharges == 1 && dashCharges == 1) newSprite = ability11;
            else if (djCharges == 1 && dashCharges == 2) newSprite = ability12;
            else if (djCharges == 2 && dashCharges == 0) newSprite = ability20;
            else if (djCharges == 2 && dashCharges == 1) newSprite = ability21;
            else if (djCharges == 2 && dashCharges == 2) newSprite = ability22;


            abilities.overrideSprite = newSprite;
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

        private void UpdateIndicatorDisplay(CharacterHandler player, GameObject rockInd,
            GameObject paperInd, GameObject scissorsInd, Image selectedProjectile)
        {
            ProjectileTypes pType = player.sB.GetSelectedWeapon();

            switch (pType)
            {
                case ProjectileTypes.ROCK:
                    rockInd.SetActive(true);
                    paperInd.SetActive(false);
                    scissorsInd.SetActive(false);
                    selectedProjectile.sprite = rock;
                    break;

                case ProjectileTypes.PAPER:
                    rockInd.SetActive(false);
                    paperInd.SetActive(true);
                    scissorsInd.SetActive(false);
                    selectedProjectile.sprite = paper;
                    break;

                case ProjectileTypes.SCISSORS:
                    rockInd.SetActive(false);
                    paperInd.SetActive(false);
                    scissorsInd.SetActive(true);
                    selectedProjectile.sprite = scissors;
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

            if (team1Points.text != points1) team1Points.text = points1;
            if (team2Points.text != points2) team2Points.text = points2;
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
            string newText = "Round " + matchManager.currentRound.ToString("00");
            if (round.text != newText) round.text = newText;
        }

        private void UpdateZoneTimer()
        {
            zoneTimer.text = "Time until next zone: " + matchManager.zoneChangeTimer.ToString("00");
        }

        private void UpdateZoneOccupant()
        {
            zoneOccupant.text = "Current occupant: " +
                matchManager.zonesList[matchManager.activeZone].currentOccupant.ToString();
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