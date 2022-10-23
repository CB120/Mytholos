using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebrisColumn
{
    public DebrisSFX[] debrisSFX;
}

public class DebrisSFXManager : MonoBehaviour
{
    // Properties
    public int regionWidth;
    public float debrisVolumeScalar = 1f;


    // Variables


    // References
    [Header("In-Prefab References")]
    public DebrisColumn[] debrisColumns;

    [Header("Element SOs")] 
    public SO_Element fireElement;
    public SO_Element electricityElement;
    public SO_Element iceElement;


    // Engine-called
    private void Awake()
    {
        SetRegionProperties();
    }


    // Methods
        // Private
    void SetRegionProperties() { 
        foreach (DebrisColumn c in debrisColumns)
        {
            foreach (DebrisSFX d in c.debrisSFX)
            {
                d.regionWidth = regionWidth;
                d.debrisVolumeScalar = debrisVolumeScalar;
                d.fireElement = fireElement;
                d.electricityElement = electricityElement;
                d.iceElement = iceElement;
            }
        }
    }
}
