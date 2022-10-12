using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodeAbility : UIMenuNode // Not to be confused with UIPartyAbility, which stores all the UI and ability related information
{
    public UIPartyManager manager;

    override public void OnNavigate(int playerNumber, Direction direction)
    {

    }

    override public void OnAction(Action action, int playerNumber)
    {
        if (action == Action.Start)
            manager.TryStartGame();

        int abilityIndex = -1;

        switch (action)
        {
            case Action.North:
                abilityIndex = 0;
                break;
            case Action.West:
                abilityIndex = 1;
                break;
            case Action.South:
                abilityIndex = 2;
                break;
            default:
                break;
        }

        if (abilityIndex < 0) return;

        if (GetComponent<UIPartyAbility>().isGreyedOut) return; //SFX here

        manager.AssignAbility(playerNumber, abilityIndex, GetComponent<UIPartyAbility>().ability);
    }
}

// Needs to get disabled when selected, when myth sheet is populated and already selected
// Needs to get undisabled when another ability is assigned to the button this one was