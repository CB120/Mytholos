using UnityEngine;

public class BeamHead : MonoBehaviour
{
    public Transform BeamHeadPosition;

    void Update()
    {
        transform.position = BeamHeadPosition.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name == "BeamHead")
        {
            // TODO: Commented out for now
            // Destroy(transform.parent.gameObject);
        }
    }
}
