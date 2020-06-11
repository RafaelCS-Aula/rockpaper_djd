using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPiece : MonoBehaviour, IComparable<ArenaPiece>
{

    private List<ConnectorGroup> sideConnectorGroups;
    public int largestGroupCount;
    public int smallestGroupCount;


    public void Setup()
    {
        sideConnectorGroups.Sort();
        largestGroupCount = sideConnectorGroups[0].connectorCount;
        smallestGroupCount = 
            sideConnectorGroups[sideConnectorGroups.Count - 1].connectorCount;
    }

    public bool isFull()
    {
        foreach(ConnectorGroup c in sideConnectorGroups)
            if(!c.isUsed)
                return false;
        return true;

    }

    public (bool valid, Transform positionRot) EvaluatePiece(ArenaPiece other)
    {
        //TODO: Check for intersecting geometry

        foreach(ConnectorGroup co in other.sideConnectorGroups)
        {
            foreach(ConnectorGroup ct in this.sideConnectorGroups)
            {
                if(!co.isUsed && !ct.isUsed && 
                    co.connectorCount == ct.connectorCount)
                    {
                        co.isUsed = true;
                        return (true, TransformPiece(ct, co, other));
                    }
                

                else 
                    return (false, null);
            }

        }
        return (false, null);
    }

    /// <summary>
    /// Gets the correct position and rotation of the other piece so its
    /// connector group matches this piece's.
    /// </summary>
    /// <param name="myConnectorGroup"></param>
    /// <param name="otherConnectorGroup"></param>
    /// <param name="otherPiece"></param>
    /// <returns></returns>
    private Transform TransformPiece(ConnectorGroup myConnectorGroup, ConnectorGroup otherConnectorGroup, ArenaPiece otherPiece)
    {
        otherPiece.transform.position = myConnectorGroup.transform.position;
        otherPiece.transform.position -= (
            otherConnectorGroup.transform.position - 
            otherPiece.transform.position);

        

        Vector3 axi = 
            otherConnectorGroup.orientation == ConnectorGroupTypes.SIDE ?
                transform.up : transform.forward;
        
        //TODO: Get angle between the heading vectors of the connectorgroups
        //otherPiece.transform.RotateAround(otherConnectorGroup, axi,  )

        
    }

    public int CompareTo(ArenaPiece other)
    {
        if (this.sideConnectorGroups[0].connectorCount >
            other.sideConnectorGroups[0].connectorCount)
            return -1;
        else if (this.sideConnectorGroups[0].connectorCount <
            other.sideConnectorGroups[0].connectorCount)
            return 1;
        else
            return 0;
    }
}
