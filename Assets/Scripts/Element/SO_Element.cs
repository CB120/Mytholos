using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Element", order = 1)]
public class SO_Element : ScriptableObject
{
    // TODO: Not ideal to be storing this here, elements shouldn't know about debris
    public GameObject debrisBehavioursPrefab;

    public Color color;
    
    // TODO: Move element icon here too

    public List<SO_Element> strongAgainst = new();
}
