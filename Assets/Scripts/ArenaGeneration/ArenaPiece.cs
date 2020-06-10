using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPiece : MonoBehaviour, IComparable<ArenaPiece>
{

    private List<ConnectorGroup> connectorGroups;
    public int largestGroupCount;
    public int smallestGroupCount;
    public bool isFull = false;

    public void Setup()
    {
        connectorGroups.Sort();
        largestGroupCount = connectorGroups[0].connectorCount;
        smallestGroupCount = 
            connectorGroups[connectorGroups.Count - 1].connectorCount;
    }

    public int CompareTo(ArenaPiece other)
    {
        if (this.connectorGroups[0].connectorCount >
            other.connectorGroups[0].connectorCount)
            return -1;
        else if (this.connectorGroups[0].connectorCount <
            other.connectorGroups[0].connectorCount)
            return 1;
        else
            return 0;
    }
}
