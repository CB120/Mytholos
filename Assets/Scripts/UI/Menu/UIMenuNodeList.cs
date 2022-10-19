using UnityEngine;
using UnityEngine.UI;

public class UIMenuNodeList : UIMenuNodeGraph
{
    [SerializeField] GameObject[] arrows; // Up, down

    float currentScroll = 0;
    float listHeight;
    float elementHeight;
    float spaceHeight;

    public override void UpdateCursorTransforms()
    {
        UpdateRecordOfTransforms();

        for (int i = 0; i < playerCursors.Length; i++)
        {
            if (playerCursors[i] != null)
            {
                float nodeIndex = nodes.IndexOf(playerCurrentNode[i]);
                float distanceToCurrentNode = nodeIndex * (elementHeight + spaceHeight); // Distance from top of list (at a scroll of 0)
                float boundsMin = currentScroll;
                float boundsMax = currentScroll + listHeight;

                if (distanceToCurrentNode - spaceHeight - elementHeight > boundsMax)                    // If node lies under visible list
                    SetNewScroll(distanceToCurrentNode - spaceHeight - elementHeight - listHeight);
                else if (distanceToCurrentNode < boundsMin)                                             // If node lies above visible list
                    SetNewScroll(distanceToCurrentNode);

                playerCursors[i].SetTransform(playerCurrentNode[i].GetComponent<RectTransform>(), cursorPadding); // Once list moved, THEN update cursor transform
            }
        }
    }

    void SetNewScroll(float newScroll)
    {
        if (newScroll == currentScroll) // Don't bother if nothing will change
            return;

        bool goingUp = newScroll < currentScroll; // Cursor going up or down? So that we can round list position in the direction that makes selected node most visible

        currentScroll = newScroll;
        if (currentScroll < 0)
            currentScroll = 0;
        GetComponent<RectTransform>().localPosition = new Vector2(0.0f, listHeight + (goingUp ? Mathf.CeilToInt(currentScroll) : Mathf.FloorToInt(currentScroll)));

        // Update arrows to indicate if list continues out of bounds of visible area
        UpdateArrows();
    }

    void UpdateRecordOfTransforms()
    {
        listHeight = -transform.parent.GetComponent<RectTransform>().rect.y;
        elementHeight = -nodes[0].GetComponent<RectTransform>().rect.y * 2.0f;
        spaceHeight = GetComponent<VerticalLayoutGroup>().spacing;
    }

    void UpdateArrows()
    {
        UpdateRecordOfTransforms();

        // If first element lies out of bounds, display up arrow
        float distanceToFirstNode = 0;
        float newBoundsMin = currentScroll;
        arrows[0].SetActive(distanceToFirstNode < newBoundsMin);

        // If last element lies out of bounds, display down arrow
        float distanceToLastNode = (nodes.Count - 1) * (elementHeight + spaceHeight);
        float newBoundsMax = currentScroll + listHeight;
        arrows[1].SetActive(distanceToLastNode - spaceHeight - elementHeight > newBoundsMax);
    }

    public void EnableArrowAnimators(bool enable)
    {
        foreach (GameObject arrow in arrows)
        {
            UIAnimator animator = arrow.GetComponent<UIAnimator>();
            if (animator)
                animator.enabled = enable;
        }
    }

    public void ResetScroll()
    {
        float listHeight = -transform.parent.GetComponent<RectTransform>().rect.y;
        currentScroll = 0;
        GetComponent<RectTransform>().localPosition = new Vector2(0.0f, listHeight + Mathf.CeilToInt(currentScroll));

        UpdateArrows();
    }
}
