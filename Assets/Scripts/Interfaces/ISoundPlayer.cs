using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public interface ISoundPlayer<T> where T : ISoundHolder
    {
        T audioHandler { get; set; }


    }
}