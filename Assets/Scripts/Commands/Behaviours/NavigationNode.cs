using UnityEngine;

public class NavigationNode : MonoBehaviour
{
    public bool playerNearBy = false;
    [SerializeField] private BoxCollider nodeSpace;

    private void OnTriggerEnter(Collider other)
    {
        // Check for player
        playerNearBy = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Check for player
        playerNearBy = false;
    }

}
