using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodeMyth : UIMenuNode
{
    public UIPartyManager manager;

    override public void OnNavigate(int playerNumber, Direction direction, bool isPlayerInput)
    {
        if (manager.IsMythAlreadySelectedInATeamAndMoveAgainIfSo(this, playerNumber, direction))
        {
            base.OnNavigate(playerNumber, direction, isPlayerInput);
            return;
        } else
        {
            base.OnNavigate(playerNumber, direction, isPlayerInput);
        }
                
        manager.SelectMyth(playerNumber, this);

        // TODO: If this myth has already been selected by someone, continue navigating in that direction to the next adjacent node
    }

    override public void OnAction(Action action, int playerNumber)
    {
        //print("Node myth OnAction! (" + action + ")");

        if (action == Action.Start)
            manager.TryStartGame();

        if (action == Action.Submit)
        {
            manager.ConfirmPartyMember(playerNumber);
            UISFXManager.PlaySound("Confirm");
        }

        if (action == Action.Cancel)
        {
            manager.RemovePartyMember(playerNumber);
            UISFXManager.PlaySound("Cancel");
        }
    }
}
