using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPiece : MonoBehaviour
{

    public List<ConnectorGroup> connectorGroups;
    

    public void Setup()
    {
        connectorGroups.Sort();

    }
}
