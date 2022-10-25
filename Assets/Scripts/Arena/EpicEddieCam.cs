using System.Collections.Generic;
using Myths;
using UnityEngine;

public class EpicEddieCam : MonoBehaviour
{
    public List<Transform> positions = new List<Transform>();

    private Dictionary<PlayerParticipant, Myth> mythsInPlay = new();

    [SerializeField] private PlayerParticipantRuntimeSet playerParticipantRuntimeSet;
    [SerializeField] float offsetY;
    [SerializeField] float zoomStartOffset;
    [SerializeField] float zoomRate;
    [SerializeField] float rotationX;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] float ratioOfXToZ;
    [SerializeField] float followSpeed = 3.5f;

    Vector3 targetPos = Vector3.zero;

    private void Start()
    {
        rotationX = transform.rotation.eulerAngles.x;
    }

    // TODO: Duplicate code. See WinState.
    private void OnEnable()
    {
        playerParticipantRuntimeSet.itemAdded.AddListener(OnPlayerParticipantAdded);
        playerParticipantRuntimeSet.itemRemoved.AddListener(OnPlayerParticipantRemoved);
        
        playerParticipantRuntimeSet.items.ForEach(OnPlayerParticipantAdded);
    }

    private void OnDisable()
    {
        playerParticipantRuntimeSet.itemAdded.RemoveListener(OnPlayerParticipantAdded);
        playerParticipantRuntimeSet.itemRemoved.RemoveListener(OnPlayerParticipantRemoved);
        
        playerParticipantRuntimeSet.items.ForEach(OnPlayerParticipantRemoved);
    }

    private void OnPlayerParticipantRemoved(PlayerParticipant playerParticipant)
    {
        playerParticipant.mythInPlayChanged.RemoveListener(OnMythInPlayChanged);
    }

    private void OnPlayerParticipantAdded(PlayerParticipant playerParticipant)
    {
        playerParticipant.mythInPlayChanged.AddListener(OnMythInPlayChanged);

        OnMythInPlayChanged(playerParticipant);
    }
    
    void Update()
    {
        if (positions.Count > 1)
        {
            float maxX = -1000.0f;
            float minX = 1000.0f;
            float maxZ = -1000.0f;
            float minZ = 1000.0f;

            // Find the average position of all myths
            Vector3 averagePos = Vector3.zero;
            foreach (Transform transform in positions)
            {
                averagePos += transform.position;
                if (transform.position.x > maxX) maxX = transform.position.x;
                if (transform.position.x < minX) minX = transform.position.x;
                if (transform.position.z > maxZ) maxZ = transform.position.z;
                if (transform.position.z < minZ) minZ = transform.position.z;
            }
            averagePos /= positions.Count;

            // Find the greatest distance between myths // Scuffed rectangle mode
            float greatestDistance = (maxX - minX) > (maxZ - minZ) * ratioOfXToZ ? (maxX - minX) : (maxZ - minZ) * ratioOfXToZ;

            // Find target position based on all known variables
            float cameraDistance = minZoom;
            cameraDistance = Mathf.Clamp(zoomStartOffset + (greatestDistance * zoomRate), minZoom, maxZoom);
            
            var offsetFromAverage = new Vector3(
                0,
                Mathf.Sin(rotationX * Mathf.Deg2Rad) * cameraDistance + offsetY,
                -Mathf.Cos(rotationX * Mathf.Deg2Rad) * cameraDistance
            );

            targetPos = averagePos + offsetFromAverage;
        }
    }

    private void FixedUpdate()
    {
        if (positions.Count > 1)
        {
            // Lerp position towards this target position
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        }
    }

    public void FocusOnSingleMyth(int winningTeamIndex)
    {
        // TODO: Commented out for now, was broken by the 1v1 update, and then again by the PlayerParticipantRuntimeSet
        // // We can assume (as of, uh, writing this) that element 0 and 1 of the postions list belong to team 0, and elements 2 and 3 belong to team 1
        // if (positions.Count != 4)
        // {
        //     Debug.LogWarning("Camera expected 4 myths in it's positions array, but found " + positions.Count + "!");
        // }
        //
        // positions.RemoveAt(winningTeamIndex * 2 + 1);
        // positions.RemoveAt(winningTeamIndex * 2);
        //
        // //if (positions[0].)
    }

    private void OnMythInPlayChanged(PlayerParticipant playerParticipant)
    {
        if (mythsInPlay.ContainsKey(playerParticipant))
        {
            var oldMythInPlay = mythsInPlay[playerParticipant];

            if (oldMythInPlay != null)
                positions.Remove(oldMythInPlay.transform);
        }

        var newMythInPlay = playerParticipant.MythInPlay;

        if (newMythInPlay != null)
            positions.Add(newMythInPlay.transform);
        
        mythsInPlay[playerParticipant] = newMythInPlay;
    }
}
