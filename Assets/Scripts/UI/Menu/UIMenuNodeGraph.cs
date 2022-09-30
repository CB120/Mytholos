using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuNodeGraph : MonoBehaviour
{
    public List<UIMenuNode> nodes;

    public bool displayInactiveCursors;
    public bool allowCursorsToShareANode;
    public float cursorPadding;

    public UIAnimator[] playerCursors; // Reference expected to be set in-editor
    public UIMenuNode[] playerCurrentNode;

    //private void Update()
    //{
    //    print("No. of nodes: " + nodes.Count + ", player 0 here? " + (playerCurrentNode[0] != null) + ", player 1 here? " + (playerCurrentNode[1] != null));
    //}

    virtual public UIMenuNodeGraph ParseNavigation(UIMenuNode.Direction direction, int playerNumber)
    {
        int index = (int)direction;

        if (playerCurrentNode[playerNumber] == null)
        {
            print("Player " + playerNumber + " has no 'current node' in this graph!");
            return this;
        }

        GameObject[] adjacent = playerCurrentNode[playerNumber].adjacent; // Hm...

        if (adjacent[index] == null) return this; // If nothing in that direction, return

        UIMenuNode node = adjacent[index].GetComponent<UIMenuNode>(); // If there's a node in that direction, navigate to it and return its parent
        if (node != null)
        {
            Navigate(node, playerNumber, direction);
            return this;
        }

        UIMenuNodeGraph graph = adjacent[index].GetComponent<UIMenuNodeGraph>(); // If there's a new graph in that direction, navigate into it and return that graph
        if (graph != null) return Navigate(graph, playerNumber, direction);

        UIMenuNodeSplitGraph splitGraph = adjacent[index].GetComponent<UIMenuNodeSplitGraph>(); // If there's a split graph, resolve navigation and return the nested graph
        if (splitGraph != null) return Navigate(splitGraph, playerNumber, direction);

        return this;
    }

    virtual public void Navigate(UIMenuNode node, int playerNumber, UIMenuNode.Direction direction)
    {
        //print("Navigating to a NODE");
        if (!allowCursorsToShareANode && NodeIsAlreadyOccupied(node, playerNumber))
        {
            playerCurrentNode[playerNumber] = node;
            int index = (int)direction;
            ParseNavigation(direction, playerNumber);
        }
        else
        {
            playerCurrentNode[playerNumber] = node;
            node.OnNavigate(playerNumber, direction); // Perform any behaviour that might occur when moving to this node
            UpdateCursorTransforms();
        }
    }

    UIMenuNodeGraph Navigate(UIMenuNodeGraph graph, int playerNumber, UIMenuNode.Direction direction) // Return type is unneccesary
    {
        //print("Navigating to a GRAPH");
        PlayerExitGraph(playerNumber);
        graph.PlayerEnterGraph(playerNumber);
        graph.Navigate(graph.playerCurrentNode[playerNumber], playerNumber, direction);
        return graph; // Leave this graph, and in the adjacent graph, navigate to the current node
    }

    UIMenuNodeGraph Navigate(UIMenuNodeSplitGraph splitGraph, int playerNumber, UIMenuNode.Direction direction)
    {
        //print("Navigating to a SPLIT GRAPH");
        return Navigate(splitGraph.subGraphs[playerNumber], playerNumber, direction); // Navigate to nested graph that belongs to the specified player
    }

    virtual public void ParseAction(UIMenuNode.Action action, int playerNumber)
    {
        playerCurrentNode[playerNumber].OnAction(action, playerNumber); // Perform behaviour specified by that node for that action
    }

    public void PlayerEnterGraph(int playerNumber)
    {
        //print("Player " + playerNumber + " is entering " + gameObject.name);

        if (playerCursors.Length <= playerNumber) return;
        if (playerCursors[playerNumber] == null) return;

        playerCursors[playerNumber].SetHidden(false);
        playerCursors[playerNumber].isPlaying = true;
    }

    public void PlayerExitGraph(int playerNumber)
    {
        //print("Player " + playerNumber + " is exiting " + gameObject.name);

        if (playerCursors.Length <= playerNumber) return;
        if (playerCursors[playerNumber] == null) return;

        playerCursors[playerNumber].isPlaying = false;
        playerCursors[playerNumber].SetHidden(!displayInactiveCursors);
    }

    public void UpdateCursorTransforms()
    {
        for (int i = 0; i < playerCursors.Length; i++)
        {
            if (playerCursors[i] != null)
                playerCursors[i].SetTransform(playerCurrentNode[i].GetComponent<RectTransform>(), cursorPadding); // Update cursor transform
        }
    }

    public void InitialiseCursorsAndStartingNodes(bool areCursorsActive)
    {
        int count = 0;

        for (int i = 0; i < playerCursors.Length; i++)
        {
            if (playerCursors[i] != null)
            {
                playerCurrentNode[i] = nodes[count];
                if (!allowCursorsToShareANode)
                    count++;

                //print("Player cursor " + i + " valid? " + (playerCursors[i] != null));
                //print("Player current node " + i + " valid? " + (playerCurrentNode[i] != null));
                //print("Player current node " + i + "'s rectTransform valid? " + (playerCurrentNode[i].GetComponent<RectTransform>() != null));

                playerCursors[i].SetTransform(playerCurrentNode[i].GetComponent<RectTransform>(), cursorPadding); // Update cursor transform
                playerCursors[i].isPlaying = areCursorsActive;
                playerCursors[i].SetHidden(!(areCursorsActive || displayInactiveCursors));
                //playerCurrentNode[i].OnNavigate(i, UIMenuNode.Direction.Right); // Is this safe?
            }
        }
    }

    bool NodeIsAlreadyOccupied(UIMenuNode node, int playerNumber)
    {
        for (int i = 0; i < playerCurrentNode.Length; i++)
        {
            if (i != playerNumber && playerCurrentNode[i] != null)      // Don't check our own player's current node, as we cannot block ourselves
            {
                if (node == playerCurrentNode[i])                       // If the node we're moving to is another player's current node, than return true
                    return true;
            }
        }

        return false;
    }
}
