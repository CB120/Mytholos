using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Ability", order = 1)]
public class SO_Ability : ScriptableObject
{
    public SO_Element element;

    [Header("Description")]
    public new string name;
    public string description;

    [Header("Properties")]
    public float damage;
    public float healing;
    public float stamina;
    public float chargeTime;
    public float performTime;
    public bool isRanged;


    [Header("Spawning Properties")]
    public bool spawnInWorldSpace;
    public Vector3 relativeSpawnPosition;

    [FormerlySerializedAs("ability")] public GameObject abilityPrefab;
    public Collider colliderPrefab;
}
