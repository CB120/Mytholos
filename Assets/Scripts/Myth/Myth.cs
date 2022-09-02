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
    public float stamina;
    public float speed;
    public float acceleration;
    public float health;

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
        print("North Pressed");
        GameObject obj = Instantiate(northAbility.ability.gameObject, this.gameObject.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.gameObject.transform);
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

    public virtual void OnTakeDamage(float damage)//When a myth Recieves Damage
    {
        health -= damage;
    }
}
