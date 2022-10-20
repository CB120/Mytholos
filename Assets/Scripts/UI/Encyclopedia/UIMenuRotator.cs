using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIMenuRotator : UIMenuNodeGraph
{
    // TODO: Ask eddie how to incorporate this into the already existing environment.
    private void onNavigate(UIMenuNode.Direction direction)
    {
        if (direction == UIMenuNode.Direction.Left)
        {
            UIMenuNode firstNode = nodes[0];
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                nodes[i].transform.position = nodes[i + 1].transform.position;
                nodes[i] = nodes[i + 1];
            }
            nodes[nodes.Count - 1] = firstNode;
            for (int i = 0; i < nodes.Count; i++) { 
                Debug.Log(nodes[i]);
            }
        }

        if(direction == UIMenuNode.Direction.Right)
        {
            UIMenuNode lastNode = nodes[nodes.Count - 1];
            nodes.Insert(0, lastNode);
            nodes.RemoveAt(nodes.Count - 1);
            for (int i = 0; i < nodes.Count; i++)
            {
                Debug.Log(nodes[i]);
            }
        }
    }
}
