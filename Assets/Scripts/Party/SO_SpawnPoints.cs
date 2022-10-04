using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/SpawnLocations", order = 1)]
public class SO_SpawnPoints : ScriptableObject
{
    public List<Vector3> SpawnLocations;

}
