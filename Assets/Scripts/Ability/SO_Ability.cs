using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Ability //enter abilities as we go here
{
    DebugNorth,
    DebugWest,
    DebugSouth,
    Dodge
}



[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Ability", order = 1)]
public class SO_Ability : ScriptableObject
{
    public E_Ability ability;
    public SO_Element element;

    public float damage;
    public float healing;
}
