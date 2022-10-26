using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIMenuRotator : UIMenuNodeGraph
{

    [SerializeField] TextMeshProUGUI selectedBookName;
    private void Awake()
    {
        updatePositions(); // Get the positions of the left and right adjacent books for each node.
    }

    private void updatePositions()
    {
        if (nodes[0].GetComponent<UINodeBook>() == null) { Debug.Log("There is no UINodeBook script on this object");  return; }
        for (int i = 0; i < nodes.Count; i++)
        {
            if(playerCurrentNode[0] == nodes[i])
            {
                selectedBookName.text = nodes[i].GetComponent<UINodeBook>().book.bookName;
                nodes[i].GetComponent<UINodeBook>().isSelected = true;
            }
            else
            {
                nodes[i].GetComponent<UINodeBook>().isSelected = false;
            }
            if (nodes[i].adjacent[2] != null)
            {
                nodes[i].GetComponent<UINodeBook>().leftAdjacent = nodes[i].adjacent[3].transform.localPosition;
            }
            if (nodes[i].adjacent[3] != null)
            {
                nodes[i].GetComponent<UINodeBook>().rightAdjacent = nodes[i].adjacent[2].transform.localPosition;
            }
        }
    }
    public override void Navigate(UIMenuNode node, int playerNumber, UIMenuNode.Direction direction, bool isPlayerInput)
    {
        base.Navigate(node, playerNumber, direction, isPlayerInput);
        
        if (direction == UIMenuNode.Direction.Left) // Replace the current books position with the position of its left-adjacent book. Repeat for every book.
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                nodes[i].transform.localPosition = nodes[i].GetComponent<UINodeBook>().rightAdjacent;
            }
            updatePositions();
        }
        if(direction == UIMenuNode.Direction.Right) // Replace the current books position with the position of its right-adjacent book. Repeat for every book.
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].transform.localPosition = nodes[i].GetComponent<UINodeBook>().leftAdjacent;
            }
            updatePositions();
        }
    }
}
