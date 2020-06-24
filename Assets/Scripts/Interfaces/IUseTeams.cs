using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.Interfaces
{
    public interface IUseTeams
    {
        int teamID { get; set; }

        void InteractFriend(IUseTeams other);
        void InteractEnemy(IUseTeams other);


    }
}