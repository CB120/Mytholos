using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBody : MonoBehaviour
{
    private BeamExtender BeamExtender;
    private BeamSettingsConduit BeamConduit;
    private BeamAttack Beam;

    private void Start()
    {
 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.GetComponent<Character>() != null)
        {
            Character character = other.transform.gameObject.GetComponent<Character>();
            character.TakeDamage(3.0f);
        }
    }
}
