using System;

namespace RPS_DJDIII.Assets.Scripts.Enums
{
    /// <summary>
    /// The 3 kinds of projectiles and ammo
    /// </summary>
    [Flags]
    public enum ProjectileTypes
    {
        DEFAULT = 0,
        ROCK = 1,
        PAPER = 2,
        SCISSORS = 4
    }
}