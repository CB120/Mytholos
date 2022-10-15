using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuNodeList : UIMenuNodeGraph
{
    [SerializeField] GameObject[] arrows; // up, down
    float currentScroll = 0;

    // TODO: Make work...
    //public override void UpdateCursorTransforms()
    //{
    //    float listHeight = -transform.parent.GetComponent<RectTransform>().rect.y;
    //    float padding = transform.parent.GetComponent<RectMask2D>().softness.y;
    //    float elementHeight = -nodes[0].GetComponent<RectTransform>().rect.y;
    //    float spaceHeight = GetComponent<VerticalLayoutGroup>().spacing;
    //    float cursorHeight = playerCursors[0] != null ? -playerCursors[0].GetComponent<RectTransform>().rect.y : -playerCursors[1].GetComponent<RectTransform>().rect.y;

    //    print("List: " + listHeight + ", Element: " + elementHeight + ", Space " + spaceHeight + ", Cursor: " + cursorHeight + "...");

    //    for (int i = 0; i < playerCursors.Length; i++)
    //    {
    //        if (playerCursors[i] != null)
    //        {
    //            float nodeIndex = nodes.IndexOf(playerCurrentNode[i]);
    //            float distanceToCurrentNode = (elementHeight + spaceHeight) * nodeIndex + (elementHeight / 2); // distance from a scroll of 0

    //            if (distanceToCurrentNode > (listHeight - (cursorHeight / 2.0f) - padding + currentScroll)) // Node lies under visible list
    //            {
    //                currentScroll = distanceToCurrentNode - (listHeight - (cursorHeight / 2.0f) - padding + currentScroll);
    //                GetComponent<RectTransform>().localPosition = new Vector2(0.0f, listHeight + Mathf.FloorToInt(currentScroll));
    //            }
    //            else if (distanceToCurrentNode < ((cursorHeight / 2.0f) + padding + currentScroll)) // Node lies above visible list
    //            {
    //                currentScroll = distanceToCurrentNode - ((cursorHeight / 2.0f) + padding); //(elementHeight > cursorHeight ? elementHeight / 2.0f : cursorHeight / 2.0f);
    //                GetComponent<RectTransform>().localPosition = new Vector2(0.0f, listHeight + Mathf.CeilToInt(currentScroll));
    //            }

    //            playerCursors[i].SetTransform(playerCurrentNode[i].GetComponent<RectTransform>(), cursorPadding); // Update cursor transform

    //            print("...NodeIndex: " + nodeIndex + ", Distance: " + distanceToCurrentNode + ", NewPosition: " + GetComponent<RectTransform>().localPosition);
    //        }
    //    }

    //    // If first element lies out of bounds, display up arrow
    //    arrows[0].SetActive(currentScroll > 0.0f);

    //    // If last element lies out of bounds, display down arrow
    //    float distanceToLastNode = ((elementHeight + spaceHeight) * (nodes.Count - 1) + (elementHeight / 2));
    //    float boundsMax = (listHeight - (cursorHeight / 2.0f) - padding + currentScroll);
    //    arrows[1].SetActive(distanceToLastNode > boundsMax);
    //}
}
