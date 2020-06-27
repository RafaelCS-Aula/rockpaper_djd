using UnityEngine;
using RPS_DJDIII.Assets.Scripts.Enums;
using RPS_DJDIII.Assets.Scripts.Interfaces;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    /// <summary>
    /// Class for zones, objectives in certain game modes
    /// </summary>
    public class ZoneBehaviour : MonoBehaviour, IArenaInitializable
    {


        [HideInInspector] public ZoneOccupants currentOccupant;
        public bool isInitialized {get; set;}

        private bool team1Inside;
        private bool team2Inside;

        private void Awake() 
        {
            isInitialized = false;    
        }
        public void Initialize()
        {
            team1Inside = false;
            team2Inside = false;

            isInitialized = true;
        }

        /// <summary>
        /// Update infro based on what player is inside the zone
        /// </summary>
        private void UpdateOccupants()
        {
            if (team1Inside && team2Inside) currentOccupant =
                       ZoneOccupants.CONTESTED;
            else if (team1Inside) currentOccupant = ZoneOccupants.TEAM1;
            else if (team2Inside) currentOccupant = ZoneOccupants.TEAM2;
            else currentOccupant = ZoneOccupants.NONE;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                TeamMemberBehaviour colTB = 
                    col.gameObject.GetComponent<TeamMemberBehaviour>();

                if (colTB.myTeam == 1) team1Inside = true;
                if (colTB.myTeam == 2) team2Inside = true;

                UpdateOccupants();
            }
        }
        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                TeamMemberBehaviour colTB = 
                    col.gameObject.GetComponent<TeamMemberBehaviour>();

                if (colTB.myTeam == 1) team1Inside = false;
                if (colTB.myTeam == 2) team2Inside = false;

                UpdateOccupants();
            }
        }
    }
}