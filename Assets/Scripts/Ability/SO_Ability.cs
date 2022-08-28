using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Ability //enter abilities as we go here
{
    Placeholder
}

[System.Serializable]
public class Command
{
    public Myth target; //target Myth for this Command
    //not sure what else to put here, @Jack
}


[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Ability", order = 1)]
public class SO_Ability : ScriptableObject
{
    public float damage;
    public float healing;
    public E_Element elementalType;
    public E_Ability abilityType;
}
