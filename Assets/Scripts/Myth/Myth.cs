using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Myth //One per Myth, just so we don't need to remember int IDs. In alphabetical order
{
    Arnkerrth,
    Dybbuk,
    Enfield,
    Golem,
    Gossan,
    Kelpie,
    Khepra,
    Wolper
}

public class Myth : MonoBehaviour
{
    //Properties


    //Variables
    List<Command> commandQueue = new List<Command>();


    //References
    SO_Ability[] abilities = { null, null, null, null };


    //Input-called
    public virtual void OnYPress()
    {

    }

    public virtual void OnXPress()
    {

    }

    public virtual void OnAPress()
    {

    }

    public virtual void OnBPress()
    {

    }
}
