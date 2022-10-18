using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodePartyMember : UIMenuNode
{
    public UIPartyManager manager;

    override public void OnNavigate(int playerNumber, Direction direction)
    {
        base.OnNavigate(playerNumber, direction);
        manager.SelectTeamMember(playerNumber, this);
    }

    override public void OnAction(Action action, int playerNumber)
    {
        if (action == Action.Start)
            manager.TryStartGame();

        if (action == Action.Cancel)
            manager.RemovePartyMember(playerNumber);
    }
}