using UnityEngine;

public class BeamExtender : MonoBehaviour
{
    private Transform BeamTransform;

    public bool AtMaxRange;

    private float MaxRange;

  
    void Start()
    {
        BeamTransform = gameObject.transform;
    }

    void Update()
    {    
        if (!AtMaxRange)
            BeamTransform.localScale += (new Vector3(0, 0, MaxRange)) * Time.deltaTime;

        if (BeamTransform.localScale.z > MaxRange)
            AtMaxRange = true;
    }

    public void SetMaxRange(float Length)
    {
        MaxRange = Length;
    }
}
