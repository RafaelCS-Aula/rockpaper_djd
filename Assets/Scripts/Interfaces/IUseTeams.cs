using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public interface IUseTeams
    {
        int teamID { get; set; }

        void InteractFriend(IUseTeams other);
        void InteractEnemy(IUseTeams other);


    }
}