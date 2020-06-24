using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.Interfaces
{
    public interface IDataUser<T> where T : ScriptableObject
    {

        T DataHolder { get; set; }

        void GetData();

    }
}