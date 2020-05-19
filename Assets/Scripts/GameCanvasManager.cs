using UnityEngine;
using UnityEngine.UI;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private PlayerInput player1;
    [SerializeField] private PlayerInput player2;

    [SerializeField] private Image p1Crosshair;
    [SerializeField] private Image p2Crosshair;

    [SerializeField] private Sprite aim00;
    [SerializeField] private Sprite aim01;
    [SerializeField] private Sprite aim02;
    [SerializeField] private Sprite aim10;
    [SerializeField] private Sprite aim11;
    [SerializeField] private Sprite aim12;
    [SerializeField] private Sprite aim20;
    [SerializeField] private Sprite aim21;
    [SerializeField] private Sprite aim22;


    private void Update()
    {
        UpdateCrosshair(player1, p1Crosshair);
        UpdateCrosshair(player2, p2Crosshair);
    }

    private void UpdateCrosshair(PlayerInput player, Image crosshair)
    {
        if (player.doubleJumpCharges == 0 && player.dashCharges == 0) crosshair.overrideSprite = aim00;
        else if (player.doubleJumpCharges == 0 && player.dashCharges == 1) crosshair.overrideSprite = aim01;
        else if (player.doubleJumpCharges == 0 && player.dashCharges == 2) crosshair.overrideSprite = aim02;
        else if (player.doubleJumpCharges == 1 && player.dashCharges == 0) crosshair.overrideSprite = aim10;
        else if (player.doubleJumpCharges == 1 && player.dashCharges == 1) crosshair.overrideSprite = aim11;
        else if (player.doubleJumpCharges == 1 && player.dashCharges == 2) crosshair.overrideSprite = aim12;
        else if (player.doubleJumpCharges == 2 && player.dashCharges == 0) crosshair.overrideSprite = aim20;
        else if (player.doubleJumpCharges == 2 && player.dashCharges == 1) crosshair.overrideSprite = aim21;
        else if (player.doubleJumpCharges == 2 && player.dashCharges == 2) crosshair.overrideSprite = aim22;

    }


}
