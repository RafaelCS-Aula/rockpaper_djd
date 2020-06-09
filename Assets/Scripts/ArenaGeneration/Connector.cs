using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Connector : MonoBehaviour
{

    [Range(0, 2)]
    public float weight = 1.00f;

   // public Color groupColor;

    private void OnDrawGizmos()
    {
        //Gizmos.color = groupColor;
        Gizmos.DrawWireSphere(transform.position, 0.5f * weight);
    }
}
