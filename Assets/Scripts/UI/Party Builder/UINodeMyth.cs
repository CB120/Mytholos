using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodeMyth : UIMenuNode
{
    public UIPartyManager manager;

    override public void OnNavigate(int playerNumber, Direction direction)
    {
        manager.SelectMyth(playerNumber, this);

        // TODO: If this myth has already been selected by someone, continue navigating in that direction to the next adjacent node
    }

    override public void OnAction(Action action, int playerNumber)
    {

    }
}
