using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConnectorGroup : MonoBehaviour, IComparable<ConnectorGroup>
{

    [SerializeField] private List<Connector> _connectors;
    [SerializeField] Color gizmoColor;
    private Vector3 centreOfMass;



    public bool isUsed = false;

    public int connectorCount;

    private void Awake()
    {
        //GetUnionPoint();
        /*foreach (Connector c in _connectors)
            c.groupColor = gizmoColor;
          */  
    }

    public Vector3 GetUnionPoint()
    {
        // Formula for Weighted Average:
        // (Sum(Value * weight)) / Sum(Weights) 

        Vector3 sum = Vector3.zero;
        float sumOfWeights = 0;

        foreach (Connector c in _connectors)
        {
            sum += c.transform.position * c.weight;
            sumOfWeights += c.weight;
            
        }
        centreOfMass = sum / sumOfWeights;

        print(centreOfMass);
        return centreOfMass;
    }

    public int GetConnectorCount() => _connectors.Count;

    public int CompareTo(ConnectorGroup other)
    {
        // I want the large ones at the start of the lists
        if (this._connectors.Count > other._connectors.Count)
            return -1;
        else if (this._connectors.Count < other._connectors.Count)
            return 1;
        else
            return 0;
    }

    /* private void OnDrawGizmos()
     {
         if (Application.isPlaying)
         {
             Gizmos.color = gizmoColor;
             Vector3 sum = Vector3.zero;


             Gizmos.DrawCube(centreOfMass, new Vector3(10,10,10));

         }

     }*/
}
