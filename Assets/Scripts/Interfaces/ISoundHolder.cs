using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public interface ISoundHolder
    {

        void PlayAudio(AudioClip sound, float volume);

    }
}