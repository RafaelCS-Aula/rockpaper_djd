using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace rockpaper_djd
{
    public class UIManager : MonoBehaviour
    {
        #region Player Vars

        [SerializeField] private InputBehaviour player1;
        [SerializeField] private InputBehaviour player2;

        [SerializeField] private Image p1Crosshair;
        [SerializeField] private Image p2Crosshair;

        [SerializeField] private TextMeshProUGUI p1AmmoDisplay;
        [SerializeField] private TextMeshProUGUI p2AmmoDisplay;

        #endregion

        #region Aim Sprites

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

        #region Indicator Vars

        [SerializeField] private GameObject rockIndP1;
        [SerializeField] private GameObject paperIndP1;
        [SerializeField] private GameObject scissorsIndP1;

        [SerializeField] private GameObject rockIndP2;
        [SerializeField] private GameObject paperIndP2;
        [SerializeField] private GameObject scissorsIndP2;

        #endregion

        #region Match Vars

        [SerializeField] private MatchManager matchManager;

        [SerializeField] private TextMeshProUGUI clockText;

        #endregion

        private void Awake()
        {
            Destroy(player2.gameObject.GetComponentInChildren<AudioListener>());
        }
        private void Update()
        {
            UpdateCrosshair(player1, p1Crosshair);
            UpdateCrosshair(player2, p2Crosshair);

            UpdateAmmoDisplay(player1, p1AmmoDisplay);
            UpdateAmmoDisplay(player2, p2AmmoDisplay);

            if (player1.oldType != player1.newType) UpdateIndicator(player1,
                rockIndP1, paperIndP1, scissorsIndP1);

            if (player2.oldType != player2.newType) UpdateIndicator(player2,
                rockIndP2, paperIndP2, scissorsIndP2);

            UpdateClockDisplay();

        }

        private void UpdateCrosshair(InputBehaviour player, Image crosshair)
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

        private void UpdateAmmoDisplay(InputBehaviour player, TextMeshProUGUI ammoDisplay)
        {
            (float maxRock, float currentRock) = player.sB.GetMana(ProjectileTypes.ROCK);
            (float maxPaper, float currentPaper) = player.sB.GetMana(ProjectileTypes.PAPER);
            (float maxScissors, float currentScissors) = player.sB.GetMana(ProjectileTypes.SCISSORS);
            ammoDisplay.text = $"Cylinder (Rock) ammo: {currentRock}/{maxRock}\n" +
                $"Sphere (Paper) ammo: {currentPaper}/{maxPaper}\n" +
                $"Cube (Scissors) ammo: {currentScissors}/{maxScissors}\n";
        }

        private void UpdateIndicator(InputBehaviour player, GameObject rockInd,
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

            if ((matchManager.matchTimer % 60).ToString("00") == "60")
            {
                seconds = "00";
                minutes = Mathf.Floor((matchManager.matchTimer / 60) + 1).ToString("00");
            }
                clockText.text = minutes + ":" + seconds;
        }
    }
}