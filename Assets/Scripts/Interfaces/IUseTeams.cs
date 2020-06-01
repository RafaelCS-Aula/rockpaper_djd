using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseTeams
{
    int teamID { get; set; }

    void InteractFriend(IUseTeams other);
    void InteractEnemy(IUseTeams other);


}
