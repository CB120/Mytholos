using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicEddieCam : MonoBehaviour
{
    public List<Transform> positions = new List<Transform>();

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
            targetPos = new Vector3(averagePos.x, Mathf.Sin(rotationX * Mathf.Deg2Rad) * cameraDistance, -Mathf.Cos(rotationX * Mathf.Deg2Rad) * cameraDistance);
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
}
