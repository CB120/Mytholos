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


    // Variables


    // References
    [Header("In-Prefab References")]
    public DebrisColumn[] debrisColumns;


    // Called by other systems
    public void OnDebrisChange()
    {

    }
}
