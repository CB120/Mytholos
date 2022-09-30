using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodePartyMember : UIMenuNode
{
    public UIPartyManager manager;

    override public void OnNavigate(int playerNumber, Direction direction)
    {
        manager.SelectTeamMember(playerNumber, this);
    }

    override public void OnAction(Action action, int playerNumber)
    {

    }
}