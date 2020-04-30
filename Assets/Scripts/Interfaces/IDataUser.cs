using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataUser<T> where T : ScriptableObject
{

    T DataHolder {get; set;}
    
    void GetData();

}
