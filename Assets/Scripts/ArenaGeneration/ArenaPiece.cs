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

    public (bool valid, Transform positionRot) EvaluatePiece(
        ArenaPiece other, float pieceDistance = 0.0f)
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
                        return (true, TransformPiece(
                            ct,
                            co, 
                            other, 
                            pieceDistance)
                        );
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
    private Transform TransformPiece(ConnectorGroup myConnectorGroup, ConnectorGroup otherConnectorGroup, ArenaPiece otherPiece, 
    float offset)
    {
        Transform newPieceTrn = otherConnectorGroup.transform;
        Quaternion connectorPointRotation = new Quaternion();

        // Put the other piece on my connector
        newPieceTrn.position = myConnectorGroup.transform.position;

        // temprarily revert parenting so we can move the connector
        // group and have the geometry follow.
        otherConnectorGroup.transform.SetParent(null, true);
        otherPiece.transform.SetParent(otherConnectorGroup.transform, true);

        // Have the other connector group look towards my connector group
        connectorPointRotation.SetLookRotation(
            -myConnectorGroup.transform.forward,
             otherConnectorGroup.transform.up);

        // Apply the rotation acquired above
        newPieceTrn.rotation = connectorPointRotation;

        // move the connectors away from each other based on an offset
        newPieceTrn.position -= newPieceTrn.forward * offset;

        // get the parenting back to normal to safeguard future transformations.
        otherConnectorGroup.transform.SetParent(null, true);
        otherPiece.transform.SetParent(otherConnectorGroup.transform, true);   	

        return newPieceTrn;
        
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
