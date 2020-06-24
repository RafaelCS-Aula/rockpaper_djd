using RPS_DJDIII.Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    public class TeamMemberBehaviour : MonoBehaviour
    {
        [SerializeField] private bool useSetTeam;
        [SerializeField] private int setTeamID;

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
