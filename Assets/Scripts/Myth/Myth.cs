using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Myth
{
    Beetle,
    Fox,
    Golem,
    Horse,
    Lizard,
    Mask,
    Snake,
    Stag
}

public class Myth : MonoBehaviour
{
    //Properties
    public E_Myth myth;


    //Variables
    List<Command> commandQueue = new List<Command>();


    //References
    public SO_Ability northAbility;
    public SO_Ability westAbility;
    public SO_Ability southAbility;
    public SO_Ability eastAbility;


    //Input-called
    public virtual void OnNorthPress() //Xbox -> Y | PlayStation -> Triangle | Switch -> X
    {

    }

    public virtual void OnWestPress() //Xbox -> X | PlayStation -> Square | Switch -> Y
    {

    }

    public virtual void OnSouthPress() //Xbox -> A | PlayStation -> X | Switch -> B
    {

    }

    public virtual void OnEastPress() //Dodge? | Xbox -> B
    {

    }
}
