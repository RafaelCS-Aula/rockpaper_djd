using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPS_DJDIII.Assets.Scripts.Enums;

namespace RPS_DJDIII.Assets.Scripts.ArenaGeneration
{

    /// <summary>
    /// Connectors are used to join ArenaPieces together to form an arena
    /// </summary>
    public class Connector : MonoBehaviour, IComparable<Connector>
    {
        /// <summary>
        /// Where would another connector join with this one.
        /// </summary>
        [SerializeField] public ConnectorOrientations orientation;

        /// <summary>
        /// The direction where this connector is "looking"
        /// </summary>
        public Vector3 heading => orientation == ConnectorOrientations.SIDE ?  transform.forward : orientation == ConnectorOrientations.TOP ? transform.up : -transform.up;

        /// <summary>
        /// Is another connector already connected to this one?
        /// </summary>
        [HideInInspector] public bool isUsed = false;

        /// <summary>
        /// Defines with what other conenctors this one can stick with.
        /// </summary>
        public int pins = 0;

        /// <summary>
        /// Visual representation of the size of connectors, 
        /// doesn't affect generation
        /// </summary>
        [SerializeField]private  float _pinSpacing = 0.5f;

        /// <summary>
        /// Color of the conenctor gizmo, for organizational purposes.
        /// </summary>
        [SerializeField] private Color _gizmoColor;

        /// <summary>
        /// Compare connectors based on their Pins, Sorts in descending order
        /// </summary>
        /// <param name="other">The other connector</param>
        /// <returns></returns>
        public int CompareTo(Connector other)
        {
            // I want the large ones at the start of the lists
            if (this.pins > other.pins)
                return -1;
            else if (this.pins < other.pins)
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

            for(float i = 0 - pins / 2; i <=  pins / 2; i++)
            {
                if(pins % 2 == 0 && i == 0)
                {

                    continue;

                }
                //pos.x = transform.position.x + (i * connectorSpacing);
                pos = transform.position + transform.right * i * _pinSpacing;
                //pos.z = transform.position.z * transform.right.z  + (i * connectorSpacing);
                if(orientation == ConnectorOrientations.SIDE)
                    Gizmos.DrawWireCube(pos , new Vector3(
                        _pinSpacing,
                        _pinSpacing,
                        _pinSpacing) );
                else
                    Gizmos.DrawWireSphere(pos, _pinSpacing / 2);
            }
        

            
        }
    }
}