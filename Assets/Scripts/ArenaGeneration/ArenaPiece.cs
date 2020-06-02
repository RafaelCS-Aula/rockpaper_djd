using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPiece : MonoBehaviour
{
    [System.Serializable]public class ConnectorSets
    {
        public List<Transform> ConnectorPositions;
        public Color gColor;

        ConnectorSets() => ConnectorPositions = new List<Transform>();
    }

    [SerializeField] private List<ConnectorSets> sideConnectorSets;
    [SerializeField] private List<ConnectorSets> topConnectorSets;
    [SerializeField] private List<ConnectorSets> bottomConnectorSets;

    // Make SpawnerBehaviour
    // private List<SpawnerBehaviour> spawners;
   
    



    private void Awake()
    {
        //spawners = GetComponentInChildren<SpawnerBehaviour>();

        // TO GIVE GIZMOS COLOR
        foreach (ConnectorSets c in sideConnectorSets)
            c.gColor = Random.ColorHSV();
        foreach (ConnectorSets c in topConnectorSets)
            c.gColor = Random.ColorHSV();
        foreach (ConnectorSets c in bottomConnectorSets)
            c.gColor = Random.ColorHSV();
    }

    private void OnDrawGizmos()
    {
        foreach (ConnectorSets c in topConnectorSets)
        {
            Gizmos.color = c.gColor;
            if (c != null)
                foreach (Transform t in c.ConnectorPositions)
                {

                    if (t != null)
                        Gizmos.DrawCube(t.position, Vector3.one);

                }
        }


        foreach (ConnectorSets c in sideConnectorSets)
        {
            Gizmos.color = c.gColor;

            if (c != null)
                foreach (Transform t in c.ConnectorPositions)
                {
                    
                    if (t != null)
                        Gizmos.DrawSphere(t.position, 0.5f);

                }
        }


        foreach (ConnectorSets c in bottomConnectorSets)
        {
            Gizmos.color = c.gColor;
            if (c != null)
                foreach (Transform t in c.ConnectorPositions)
                {

                    if (t != null)
                        Gizmos.DrawWireCube(t.position, Vector3.one);

                }
        }

    }
}
