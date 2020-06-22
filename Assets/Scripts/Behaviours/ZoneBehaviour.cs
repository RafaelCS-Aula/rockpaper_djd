using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public class ZoneBehaviour : MonoBehaviour
    {
        [HideInInspector] public ZoneOccupants currentOccupant;

        private bool team1Inside = false;
        private bool team2Inside = false;

        private void UpdateOccupants()
        {
            if (team1Inside && team2Inside) currentOccupant = ZoneOccupants.CONTESTED;
            else if (team1Inside) currentOccupant = ZoneOccupants.TEAM1;
            else if (team2Inside) currentOccupant = ZoneOccupants.TEAM2;
            else currentOccupant = ZoneOccupants.NONE;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                TeamMemberBehaviour colTB = col.gameObject.GetComponent<TeamMemberBehaviour>();

                if (colTB.myTeam == 1) team1Inside = true;
                if (colTB.myTeam == 2) team2Inside = true;

                UpdateOccupants();
            }
        }
        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                TeamMemberBehaviour colTB = col.gameObject.GetComponent<TeamMemberBehaviour>();

                if (colTB.myTeam == 1) team1Inside = false;
                if (colTB.myTeam == 2) team2Inside = false;

                UpdateOccupants();
            }
        }
    }
}
