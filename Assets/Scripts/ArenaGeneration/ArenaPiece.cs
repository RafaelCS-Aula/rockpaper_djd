using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPiece : MonoBehaviour, IComparable<ArenaPiece>
{

    [SerializeField] private List<ConnectorGroup> sideConnectorGroups;
    [SerializeField] private ConnectorGroup _topConnector;
    [SerializeField] private ConnectorGroup _bottomConnector;


    [HideInInspector] public bool wasAnalysed = false;
    
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
 

    public (bool hasTop, bool hasBottom) GetVerticalConnectors()
    {
        bool t = false;
        bool b = false;

        if(_topConnector != null && !_topConnector.isUsed)
            t = true;
        if(_bottomConnector != null && !_bottomConnector.isUsed)
            b = true;
        
        return (t, b);

    }

    public (bool valid, Transform position) EvaluatePieceVertical(
        ArenaPiece other, bool upper, float pieceDistance = 0.00f,
         int groupTolerance = 0)
        {
            ConnectorGroup myCon = null;
            ConnectorGroup otherCon = null;
            if(upper && other._topConnector != null)
            {
                myCon = _topConnector;
                otherCon = other._bottomConnector; 

            }
            else if(!upper && other._bottomConnector != null)
            {
                myCon = _bottomConnector;
                otherCon = other._topConnector; 

            } 

            if(!otherCon.isUsed && !myCon.isUsed && 
                otherCon.connectorCount >= myCon.connectorCount-groupTolerance 
                && otherCon.connectorCount <= myCon.connectorCount)
                    {
                        otherCon.isUsed = true;
                        myCon.isUsed = true;
                        return (true, TransformPiece(
                            myCon,
                            otherCon, 
                            other, 
                            pieceDistance));
                       
                    }
            else 
                return (false, null);

        }


    public (bool valid, Transform positionRot) EvaluatePiece(
        ArenaPiece other, float pieceDistance = 0.00f, int groupTolerance = 0)
    {
        

        List<(ConnectorGroup mine, ConnectorGroup oth)> possibleCombos =
        new List<(ConnectorGroup mine, ConnectorGroup oth)>();
        //Check for intersecting geometry?
        //Spawn the piece and have it tell if the trigger collider reports back
        // ...but what if the piece is not all in one mesh?

        foreach(ConnectorGroup co in other.sideConnectorGroups)
        {
            foreach(ConnectorGroup ct in this.sideConnectorGroups)
            {
                if(!co.isUsed && !ct.isUsed && 
                    co.connectorCount >= ct.connectorCount - groupTolerance &&
                    co.connectorCount <= ct.connectorCount)
                {
                    possibleCombos.Add((ct, co));
                }
            }

        }

        if(possibleCombos.Count > 0)
        {
            (ConnectorGroup chosenMine, ConnectorGroup chosenOther)
             choosenCombo = possibleCombos[
                 UnityEngine.Random.Range(0, possibleCombos.Count)];

            choosenCombo.chosenOther.isUsed = true;
            choosenCombo.chosenMine.isUsed = true;

            Transform trn = TransformPiece(choosenCombo.chosenMine,
            choosenCombo.chosenOther, other, pieceDistance);

            return (true, trn);
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
            -myConnectorGroup.heading,
             transform.up);

        // Apply the rotation acquired above
        newPieceTrn.rotation = connectorPointRotation;

        // move the pieces away from each other based on an offset
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
