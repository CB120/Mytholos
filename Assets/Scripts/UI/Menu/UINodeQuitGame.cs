using UnityEngine;

public class UINodeQuitGame : UIMenuNode
{
    override public void OnAction(Action action, int playerNumber)
    {
        switch (action)
        {
            case Action.Submit:
                Application.Quit();
                break;
            default:
                break;
        }
    }
}