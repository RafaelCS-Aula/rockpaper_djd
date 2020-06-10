using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPiece : MonoBehaviour, IComparable<ArenaPiece>
{

    public List<ConnectorGroup> connectorGroups;



    public void Setup()
    {
        connectorGroups.Sort();

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
