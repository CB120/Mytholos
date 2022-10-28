using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodeGraphAudio : UIMenuNodeGraph
{
    public override UIMenuNodeGraph ParseNavigation(UIMenuNode.Direction direction, int playerNumber, bool isPlayerInput)
    {
        playerCurrentNode[0].MoveOptionSlider(direction, playerNumber);
        return base.ParseNavigation(direction, playerNumber, isPlayerInput);
        
    }
}
