using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodeAbility : UIMenuNode // Not to be confused with UIPartyAbility, which stores all the UI and ability related information
{
    public UIPartyManager manager;

    //override public void OnNavigate(int playerNumber, Direction direction)
    //{

    //}

    override public void OnAction(Action action, int playerNumber)
    {
        if (action == Action.HoldCancel)
            manager.ProgressToNextStage(0);

        if (action == Action.Start)
            manager.TryStartGame();

        if (action == Action.Cancel)
        {
            manager.AssignAbility(playerNumber, -1, GetComponent<UIPartyAbility>().ability);
            UISFXManager.PlaySound("Invalid");
            return;
        }

        int abilityIndex = -1;
        string soundToBePlayed = "Nothing";

        switch (action)
        {
            case Action.North:
                abilityIndex = 0;
                soundToBePlayed = "Confirm North";
                break;
            case Action.West:
                abilityIndex = 1;
                soundToBePlayed = "Confirm West";
                break;
            case Action.South:
                abilityIndex = 2;
                soundToBePlayed = "Confirm South";
                break;
            default:
                break;
        }

        if (abilityIndex < 0) return;

        if (GetComponent<UIPartyAbility>().isGreyedOut)
        {
            UISFXManager.PlaySound("Invalid");
            return;
        }

        UISFXManager.PlaySound(soundToBePlayed); //this can't be done in the switch because you'd hear both the confirm sound and the 'Invalid' sound at once

        manager.AssignAbility(playerNumber, abilityIndex, GetComponent<UIPartyAbility>().ability);
    }
}

// Needs to get disabled when selected, when myth sheet is populated and already selected
// Needs to get undisabled when another ability is assigned to the button this one was