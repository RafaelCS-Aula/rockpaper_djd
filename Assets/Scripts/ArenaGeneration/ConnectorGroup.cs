﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConnectorGroup : MonoBehaviour, IComparable<ConnectorGroup>
{
    [SerializeField] private ConnectorGroupTypes orientation;
    public Vector3 heading => orientation == ConnectorGroupTypes.SIDE ?  transform.forward : orientation == ConnectorGroupTypes.TOP ? transform.up : -transform.up;


    public bool isUsed = false;

    public int connectorCount = 0;

    [SerializeField]private  float connectorSpacing = 0.5f;


    [SerializeField] private Color _gizmoColor;

    private void Awake()
    {
        //GetUnionPoint();
        /*foreach (Connector c in _connectors)
            c.groupColor = gizmoColor;
          */  
        
    }


    public int CompareTo(ConnectorGroup other)
    {
        // I want the large ones at the start of the lists
        if (this.connectorCount > other.connectorCount)
            return -1;
        else if (this.connectorCount < other.connectorCount)
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

         for(float i = 0 - connectorCount / 2; i <=  connectorCount / 2; i++)
         {
             if(connectorCount % 2 == 0 && i == 0)
             {

                continue;

             }
             //pos.x = transform.position.x + (i * connectorSpacing);
             pos = transform.position + transform.right * i * connectorSpacing;
             //pos.z = transform.position.z * transform.right.z  + (i * connectorSpacing);
             if(orientation == ConnectorGroupTypes.SIDE)
                Gizmos.DrawWireCube(pos , new Vector3(
                    connectorSpacing,
                    connectorSpacing,
                    connectorSpacing) );
            else
                Gizmos.DrawWireSphere(pos, connectorSpacing / 2);
         }
    

        
     }
}