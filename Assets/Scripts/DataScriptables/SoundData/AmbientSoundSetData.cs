using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.DataScriptables.SoundData
{
    [CreateAssetMenu(fileName = "AmbientSoundSet",
    menuName = "Data/Audio/Ambient Sound Set", order = 1)]
    public class AmbientSoundSetData : ScriptableObject
    {
        public AudioClip background;
        public List<AudioClip> audioLayers;
    }
}