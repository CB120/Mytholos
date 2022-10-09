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
    [Tooltip("How Fast Stat Properties Regenerate (Health, Stamina)")] public float regenSpeed;
    [Tooltip("Health, Agility, Any Arbitrary Stat Value")]  public float statIncrease; //TODO: Perhaps Move things like this to SO_ELEMENT
    public float staminaCost;
    public float chargeTime;
    public float performTime;
    public bool isRanged;
    public bool applyBuffToParty;
    public float baseKnockback;
    public float baseStun;

    [Header("Spawning Properties")]
    public bool spawnInWorldSpace;
    public Vector3 relativeSpawnPosition;
    public Vector3 relativeSpawnRotation;
    public float timeToDestroy;

    [FormerlySerializedAs("ability")] public GameObject abilityPrefab;
}
