using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class ArenaPiece : MonoBehaviour, IComparable<ArenaPiece>
{
    /// <summary>
    /// DO NOT UNDER ANY EXCUSE REMOVE THE [SERIALIZEFIELD] OR 
    /// THE INITIALIZATION FROM THESE LINES IT WILL MAKE UNITY HANG AND
    /// EAT YOUR MEMORY.
    /// </summary>
    [HideInInspector] [SerializeField] private List<Connector> sideConnectors
    = new List<Connector>();

    [HideInInspector] [SerializeField] 
    private Connector _topConnector = null;
    
    [HideInInspector] [SerializeField] 
    private Connector _bottomConnector = null;

    private bool _useRigidBody;
    [HideInInspector]  public bool wasAnalysed = false;
    
    [HideInInspector] public int ConnectorsCount;


    /// <summary>
    /// Detects the connectors, sorts them and activates the rigidbodies
    /// </summary>
    /// <param name="spawnRigid"></param>
    public void Setup(bool spawnRigid)
    {
        wasAnalysed = false;

        //Debug.Log("Using first bottom/top connectors found.");
        List<Connector> children = new List<Connector>();
        _useRigidBody = spawnRigid;
        //Detect connectors
        foreach(Connector c in GetComponentsInChildren<Connector>())
        {
            if(c.orientation == ConnectorOrientations.SIDE)
                children.Add(c);
            else if (c.orientation == ConnectorOrientations.TOP)
            {
                if(_topConnector == null)
                    _topConnector = c;
            }
            else if (c.orientation == ConnectorOrientations.BOTTOM)
            {
                if(_bottomConnector == null)
                    _bottomConnector = c;
            }


        }
        
        sideConnectors = children.Distinct().ToList();
        _useRigidBody = spawnRigid;
        sideConnectors.Sort();
        ConnectorsCount = sideConnectors.Count;
        

        foreach (Connector g in sideConnectors)
        {
            g.isUsed = false;
        }
        if(_topConnector != null)
            _topConnector.isUsed = false;
        if(_bottomConnector != null)
            _bottomConnector.isUsed = false;
        
        
        Rigidbody rb = GetComponent<Rigidbody>();

        if(rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        
        if(_useRigidBody)
        {
            rb.isKinematic = false;
            GetComponent<MeshCollider>().convex = true;
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.mass = 0;
            rb.drag = 900;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.isKinematic = true;
        }
  
        
    }

    /// <summary>
    /// Checks all connectors to see if they're already connected to another
    /// </summary>
    /// <returns> Are all the connectors in this piece used?</returns>
    public bool IsFull()
    {
        foreach(Connector c in sideConnectors)
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

   /* public (bool valid, Transform position) EvaluatePieceVertical(
        ArenaPiece other, bool upper, float pieceDistance = 0.00f,
         int groupTolerance = 0)
        {
            Connector myCon = null;
            Connector otherCon = null;
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

            if(otherCon == null || myCon == null)
                return (false, null);
                
            if(!otherCon.isUsed && !myCon.isUsed && 
                otherCon.pins >= myCon.pins-groupTolerance 
                && otherCon.pins <= myCon.pins)
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

        }*/


    public (bool valid, Transform positionRot) EvaluatePiece(
        ArenaPiece other, float pieceDistance = 0.00f, uint groupTolerance = 0)
    {
        

        List<(Connector mine, Connector oth)> possibleCombos =
        new List<(Connector mine, Connector oth)>();
        //Check for intersecting geometry?
        //Spawn the piece and have it tell if the trigger collider reports back
        // ...but what if the piece is not all in one mesh?

        foreach(Connector co in other.sideConnectors)
        {
            foreach(Connector ct in this.sideConnectors)
            {
                if(!co.isUsed && !ct.isUsed && 
                    co.pins >= ct.pins - groupTolerance &&
                    co.pins <= ct.pins)
                {
                    possibleCombos.Add((ct, co));
                }
            }

        }

        if(possibleCombos.Count > 0)
        {
            (Connector chosenMine, Connector chosenOther)
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
    private Transform TransformPiece(Connector myConnectorGroup, Connector otherConnectorGroup, ArenaPiece otherPiece, 
    float offset)
    {
        
        Transform newPieceTrn = otherConnectorGroup.transform;
        Quaternion connectorPointRotation = new Quaternion();

        // temprarily revert parenting so we can move the connector
        // group and have the geometry follow.

        otherConnectorGroup.transform.SetParent(null, true);
        otherPiece.transform.SetParent(otherConnectorGroup.transform, true);
       
        // Put the other piece on my connector
        newPieceTrn.position = myConnectorGroup.transform.position;



        if (otherConnectorGroup.orientation == ConnectorOrientations.SIDE)
        {
            // Have the other connector group look towards my connector group
            connectorPointRotation.SetLookRotation(
                -myConnectorGroup.heading,
                 transform.up);

            // Apply the rotation acquired above
            newPieceTrn.rotation = connectorPointRotation;

        }


        // move the pieces away from each other based on an offset
        newPieceTrn.position -= otherConnectorGroup.heading * offset;

        // get the parenting back to normal to safeguard future transformations.
        otherPiece.transform.SetParent(null, true);
        otherConnectorGroup.transform.SetParent(otherPiece.transform, true);
           	

        return newPieceTrn;
        
    }

    /// <summary>
    /// Order the peices by how many connectors they have
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(ArenaPiece other)
    {
        if (this.sideConnectors.Count >
            other.sideConnectors.Count)
            return -1;
        else if (this.sideConnectors.Count <
            other.sideConnectors.Count)
            return 1;
        else
            return 0;
    }
}
