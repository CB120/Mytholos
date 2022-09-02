using UnityEngine;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Ability", order = 1)]
public class SO_Ability : ScriptableObject
{
    public SO_Element element;

    public float damage;
    public float healing;
    

    public Ability ability;
    public Collider colliderPrefab;
}
