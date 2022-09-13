using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MythStatus
{
    WALKING,
    STUNNED,
    DODGE,
    ATTACKING
}

public class MovementStyleManager : MonoBehaviour
{

    private bool manualMovement = false;
    public float speed;
    public float stamina;
    public MythStatus mStatus;

}
