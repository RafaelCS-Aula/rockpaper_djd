using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public interface IDataUser<T> where T : ScriptableObject
    {

        T DataHolder { get; set; }

        void GetData();

    }
}