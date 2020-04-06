using UnityEngine;


[CreateAssetMenu(fileName = "Shooter", 
    menuName = "Agent/Shooter Data", order = 0)]
public class ShooterData : ScriptableObject
{
    [Header("Specs")]
    public float FireRate;

    [Header("Mana")]
    public bool StartWithScissor = true;
    public int StartingScissorAmmo;
    public int MaxScissorAmmo;

    public bool StartWithRock = true;
    public int StartingRockAmmo;
    public int MaxRockAmmo;

    public bool StartWithPaper = true;
    public int StartingPaperAmmo;
    public int MaxPaperAmmo;
}
