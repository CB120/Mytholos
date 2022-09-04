using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Ability", order = 1)]
public class SO_Ability : ScriptableObject
{
    public SO_Element element;

    public new string name;
    public float damage;
    public float healing;
    

    [FormerlySerializedAs("ability")] public GameObject abilityPrefab;
    public Collider colliderPrefab;
}
