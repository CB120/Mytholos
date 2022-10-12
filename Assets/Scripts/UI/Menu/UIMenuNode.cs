using UnityEngine;

public class UIMenuNode : MonoBehaviour
{
    public enum Direction { Up, Down, Right, Left }
    public enum Action { Submit, Cancel, North, South, East, West, HoldCancel, Start }

    public GameObject[] adjacent = new GameObject[4]; // Indices follow that of Direction enum

    virtual public void OnNavigate(int playerNumber, Direction direction) // Do something when a player navigates to this node, from another node
    {
        //SFX stuff
    }

    virtual public void OnAction(Action action, int playerNumber) // Do something when a player takes a particular action, while selecting this node
    {

    }
}
