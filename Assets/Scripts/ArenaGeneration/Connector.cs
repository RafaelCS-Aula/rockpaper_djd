using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Connector : MonoBehaviour, IComparable<Connector>
{
    [SerializeField] public ConnectorOrientations orientation;
    public Vector3 heading => orientation == ConnectorOrientations.SIDE ?  transform.forward : orientation == ConnectorOrientations.TOP ? transform.up : -transform.up;


    [HideInInspector] public bool isUsed = false;

    public int pins = 0;

    [SerializeField]private  float _pinSpacing = 0.5f;


    [SerializeField] private Color _gizmoColor;

    private void Awake()
    {
        //GetUnionPoint();
        /*foreach (Connector c in _connectors)
            c.groupColor = gizmoColor;
          */  
        
    }


    public int CompareTo(Connector other)
    {
        // I want the large ones at the start of the lists
        if (this.pins > other.pins)
            return -1;
        else if (this.pins < other.pins)
            return 1;
        else
            return 0;
    }

     private void OnDrawGizmos()
     {

        _gizmoColor.a = 1;
        Gizmos.color = _gizmoColor;
 
        Gizmos.DrawLine(transform.position, transform.position + heading * 2);

        Vector3 pos;

         for(float i = 0 - pins / 2; i <=  pins / 2; i++)
         {
             if(pins % 2 == 0 && i == 0)
             {

                continue;

             }
             //pos.x = transform.position.x + (i * connectorSpacing);
             pos = transform.position + transform.right * i * _pinSpacing;
             //pos.z = transform.position.z * transform.right.z  + (i * connectorSpacing);
             if(orientation == ConnectorOrientations.SIDE)
                Gizmos.DrawWireCube(pos , new Vector3(
                    _pinSpacing,
                    _pinSpacing,
                    _pinSpacing) );
            else
                Gizmos.DrawWireSphere(pos, _pinSpacing / 2);
         }
    

        
     }
}
