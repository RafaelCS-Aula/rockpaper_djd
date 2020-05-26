using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private PlayerInput player1;
    [SerializeField] private PlayerInput player2;

    [SerializeField] private Image p1Crosshair;
    [SerializeField] private Image p2Crosshair;

    [SerializeField] private TextMeshProUGUI p1AmmoDisplay;
    [SerializeField] private TextMeshProUGUI p2AmmoDisplay;

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
    }

    private void UpdateCrosshair(PlayerInput player, Image crosshair)
    {
        Sprite newSprite = crosshair.sprite;

        if (player.doubleJumpCharges == 0 && player.dashCharges == 0) newSprite = aim00;
        else if (player.doubleJumpCharges == 0 && player.dashCharges == 1) newSprite = aim01;
        else if (player.doubleJumpCharges == 0 && player.dashCharges == 2) newSprite = aim02;
        else if (player.doubleJumpCharges == 1 && player.dashCharges == 0) newSprite = aim10;
        else if (player.doubleJumpCharges == 1 && player.dashCharges == 1) newSprite = aim11;
        else if (player.doubleJumpCharges == 1 && player.dashCharges == 2) newSprite = aim12;
        else if (player.doubleJumpCharges == 2 && player.dashCharges == 0) newSprite = aim20;
        else if (player.doubleJumpCharges == 2 && player.dashCharges == 1) newSprite = aim21;
        else if (player.doubleJumpCharges == 2 && player.dashCharges == 2) newSprite = aim22;


        crosshair.overrideSprite = newSprite;
    }


    public void UpdateAmmoDisplay(PlayerInput player, TextMeshProUGUI ammoDisplay)
    {
        (float maxRock, float currentRock ) = player.sB.GetMana(ProjectileTypes.ROCK);
        (float maxPaper, float currentPaper) = player.sB.GetMana(ProjectileTypes.PAPER);
        (float maxScissors, float currentScissors) = player.sB.GetMana(ProjectileTypes.SCISSORS);
        ammoDisplay.text = $"Cylinder (Rock) ammo: {currentRock}/{maxRock}\n" +
            $"Sphere (Paper) ammo: {currentPaper}/{maxPaper}\n" +
            $"Cube (Scissors) ammo: {currentScissors}/{maxScissors}\n";
    }
}