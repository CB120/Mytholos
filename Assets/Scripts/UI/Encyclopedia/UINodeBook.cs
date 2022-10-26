using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINodeBook : UIMenuNode
{
    public Vector3 leftAdjacent;
    public Vector3 rightAdjacent;
    public string bookName;
    [SerializeField] UIEncyclopediaManager encyclopediaManager;
    

    override public void OnAction(Action action, int playerNumber)
    {
        switch (action)
        {
            case Action.Submit:
                encyclopediaManager.ParseBookInformation(this, bookName);
                encyclopediaManager.SetLibraryActive(false);
                encyclopediaManager.SetBookCanvas(true);
                break;
            default:
                break;
        }
    }
}
