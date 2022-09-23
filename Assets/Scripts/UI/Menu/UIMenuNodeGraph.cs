using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuNodeGraph : MonoBehaviour
{
    [SerializeField] List<UIMenuNode> nodes;
    public bool displayInactiveCursors;

    public UIAnimator[] playerCursors;
    public UIMenuNode[] playerCurrentNode;

    virtual public UIMenuNodeGraph ParseNavigation(UIMenuNode.Direction direction, int playerNumber)
    {
        int index = (int)direction;

        GameObject[] adjacent = playerCurrentNode[playerNumber].adjacent; // Hm...

        if (adjacent[index] == null) return null; // If nothing in that direction, return

        UIMenuNode node = adjacent[index].GetComponent<UIMenuNode>(); // If there's a node in that direction, navigate to it and return its parent
        if (node != null)
        {
            Navigate(node, playerNumber);
            return this;
        }

        UIMenuNodeGraph graph = adjacent[index].GetComponent<UIMenuNodeGraph>(); // If there's a new graph in that direction, navigate into it and return that graph
        if (graph != null) return Navigate(graph, playerNumber);

        UIMenuNodeSplitGraph splitGraph = adjacent[index].GetComponent<UIMenuNodeSplitGraph>(); // If there's a split graph, resolve navigation and return the nested graph
        if (splitGraph != null) return Navigate(splitGraph, playerNumber);

        return null;
    }

    virtual protected void Navigate(UIMenuNode node, int playerNumber)
    {
        playerCurrentNode[playerNumber] = node;
        node.OnNavigate(playerNumber); // Perform any behaviour that might occur when moving to this node
        UpdateCursorTransforms();
    }

    UIMenuNodeGraph Navigate(UIMenuNodeGraph graph, int playerNumber) // Return type is unneccesary
    {
        PlayerExitGraph(playerNumber);
        graph.PlayerEnterGraph(playerNumber);
        Navigate(graph.playerCurrentNode[playerNumber], playerNumber);
        return graph; // Leave this graph, and in the adjacent graph, navigate to the current node
    }

    UIMenuNodeGraph Navigate(UIMenuNodeSplitGraph splitGraph, int playerNumber)
    {
        return Navigate(splitGraph.subGraphs[playerNumber], playerNumber); // Navigate to nested graph that belongs to the specified player
    }

    virtual public void ParseAction(UIMenuNode.Action action, int playerNumber)
    {
        playerCurrentNode[playerNumber].OnAction(action, playerNumber); // Perform behaviour specified by that node for that action
    }

    public void PlayerEnterGraph(int playerNumber)
    {
        playerCursors[playerNumber].isPlaying = true;
    }

    public void PlayerExitGraph(int playerNumber)
    {
        playerCursors[playerNumber].isPlaying = false;
    }

    public void UpdateCursorTransforms()
    {
        for (int i = 0; i < playerCursors.Length; i++)
        {
            playerCursors[i].SetTransform(playerCurrentNode[i].GetComponent<RectTransform>()); // Update cursor transform
        }
    }
}
