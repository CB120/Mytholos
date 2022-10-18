using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuNodeGraph : MonoBehaviour
{
    public List<UIMenuNode> nodes;

    [Header("Cursor settings")]
    public bool displayInactiveCursors;
    public bool allowCursorsToShareANode;
    public float cursorPadding;

    [Header("Player action settings")]
    [SerializeField] bool navigateToLastGraphNodeOnCancel;
    [SerializeField] bool navigateToPreviousSceneOnCancel;
    [SerializeField] bool destroyAllParticipantsOnSceneCancel;
    [SerializeField] string nameOfPreviousScene;

    [Header("Scene references")]
    public UIAnimator[] playerCursors; // Reference expected to be set in-editor
    public UIMenuNode[] playerCurrentNode;
    [SerializeField] Animator transitionAnimator;

    //private void Update()
    //{
    //    print("No. of nodes: " + nodes.Count + ", player 0 here? " + (playerCurrentNode[0] != null) + ", player 1 here? " + (playerCurrentNode[1] != null));
    //}

    virtual public UIMenuNodeGraph ParseNavigation(UIMenuNode.Direction direction, int playerNumber)
    {
        int index = (int)direction;

        if (playerCurrentNode.Length <= playerNumber) return this;

        if (playerCurrentNode[playerNumber] == null)
        {
            print("Player " + playerNumber + " has no 'current node' in this graph!");
            return this;
        }

        GameObject[] adjacent = playerCurrentNode[playerNumber].adjacent; // Hm...

        if (adjacent[index] == null) return this; // If nothing in that direction, return // TODO: Let player's move left/right between team mates (during 2nd stage) using d-pad/stick

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
        //print("Player" + playerNumber + " navigating " + direction + " to node " + node.name + " in " + name);

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
        //print("Parsing action " + action + " for player " + playerNumber);

        if (action == UIMenuNode.Action.Cancel)
        {
            if (navigateToLastGraphNodeOnCancel)
            {
                Navigate(nodes[nodes.Count - 1], playerNumber, UIMenuNode.Direction.Down);
                return;
            }
        }
        else if (action == UIMenuNode.Action.HoldCancel)
        {
            if (navigateToPreviousSceneOnCancel)
            {
                if (transitionAnimator)
                {
                    foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
                    {
                        if (destroyAllParticipantsOnSceneCancel)
                            participant.DestroyParticipant();
                        else
                            participant.DisablePlayerInput(0.5f);
                    }

                    transitionAnimator.SetInteger("Direction", -1);
                    transitionAnimator.SetTrigger("Fade");
                    StartCoroutine(LoadScene(0.35f));
                }

                UISFXManager.PlaySound("Back Scene");

                return;
            }
        }

        if (playerCurrentNode.Length <= playerNumber) return;

        playerCurrentNode[playerNumber].OnAction(action, playerNumber); // Perform behaviour specified by that node for that action
    }

    IEnumerator LoadScene(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(nameOfPreviousScene);
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

    public virtual void UpdateCursorTransforms()
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
