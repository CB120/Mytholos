
using UnityEngine;

public class UINodeTab : UIMenuNode
{
    [SerializeField] UIEncyclopediaManager encyclopediaManager;
    override public void OnAction(Action action, int playerNumber)
    {
        switch (action)
        {
            case Action.Cancel:
                encyclopediaManager.SetLibraryActive(true);
                encyclopediaManager.SetBookCanvas(false);
                break;
            default:
                break;
        }
    }
}
