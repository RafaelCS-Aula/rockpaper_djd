using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundPlayer<T> where T : ISoundHolder
{
    T audioHandler { get; set; }

 
}
