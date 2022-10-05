using UnityEngine;

// This component points to several other graphs. When a player navigates from a node that points to this, we
// use the provided player number to determine which sub-graph the player should looking to move into
public class UIMenuNodeSplitGraph : MonoBehaviour
{
    public UIMenuNodeGraph[] subGraphs; // Index corresponds to player number
}
