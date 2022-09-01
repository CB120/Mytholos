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
    [SerializeField] private enum SelectedMyth
    {
        Myth1,
        Myth2
    }

    private bool manualMovement = false;
    public float speed;
    public float stamina;
    public MythStatus mStatus;

    void Update()
    {
        // If (R2 or L2 i cant fucking remember) is pressed (I think it should be toggled!!), manualMovement = true;     
    }
}
