using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Element", order = 1)]
public class SO_Element : ScriptableObject
{
    public Color color;
    
    // TODO: Move element icon here too

    public List<SO_Element> strongAgainst = new();

    // TODO: Cyclic dependency. Do we need this?
    public bool hasDebris;
}
