using UnityEngine;

public class BeamExtender : MonoBehaviour
{
    private float maxRange;

    private void Update()
    {
        if (transform.localScale.z < maxRange)
            // TODO: Extension time should be adjustable to match particle speed
            transform.localScale += new Vector3(0, 0, maxRange) * Time.deltaTime;
        if (transform.localScale.z > maxRange)
            transform.localScale = new Vector3(1, 1, maxRange);
    }

    public void SetMaxRange(float length)
    {
        maxRange = length;
    }
}
