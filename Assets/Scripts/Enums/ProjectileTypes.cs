using System;

namespace rockpaper_djd
{
    [Flags]
    public enum ProjectileTypes
    {
        DEFAULT = 0,
        ROCK = 1,
        PAPER = 2,
        SCISSORS = 4
    }
}