using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmbientSoundSet",
    menuName = "Data/Audio/Ambient Sound Set", order = 1)]
public class AmbientSoundSetData : ScriptableObject
{
    public AudioClip background;
    public List<AudioClip> audioLayers;
}
