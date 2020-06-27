using RPS_DJDIII.Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    /// <summary>
    /// Behaviour of objects that belong in a "team", a unique
    /// identification that all their components implementing IUseTeams share
    /// </summary>
    public class TeamMemberBehaviour : MonoBehaviour
    {
        [SerializeField] private bool useSetTeam;
        [SerializeField] private int setTeamID;

        /// <summary>
        /// Unique number defining the team of this object's components
        /// </summary>
        [HideInInspector] public int myTeam;

        private void Start()
        {
            if (useSetTeam)
                myTeam = setTeamID;
            else
                myTeam = Random.Range(0, 1000);

            foreach (IUseTeams t in gameObject.GetComponents<IUseTeams>())
            {
                t.teamID = myTeam;

            }
        }

    }
}
