﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSoundSet",
    menuName = "Data/Audio/Player Sound Set", order = 0)]
public class PlayerSoundSetData : ScriptableObject
{

    public AudioClip jumpSFX;
    public AudioClip doubleJumpSFX;
    public AudioClip shotSFX;
    public AudioClip dashSFX;
    public AudioClip hurtSFX;
    public AudioClip deathSFX;

}
