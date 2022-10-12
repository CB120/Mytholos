using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodeMyth : UIMenuNode
{
    public UIPartyManager manager;

    override public void OnNavigate(int playerNumber, Direction direction)
    {
        if (manager.IsMythAlreadySelectedInATeamAndMoveAgainIfSo(this, playerNumber, direction)) return;

        base.OnNavigate(playerNumber, direction); //This could cause all manner of issues if we add more stuff to UIMenuNode.OnNavigate(). Chat to Ethan if we need to change this, it'll break SFX

        manager.SelectMyth(playerNumber, this);

        // TODO: If this myth has already been selected by someone, continue navigating in that direction to the next adjacent node
    }

    override public void OnAction(Action action, int playerNumber)
    {
        if (action == Action.Start)
            manager.TryStartGame();
    }
}
