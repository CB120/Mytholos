using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Beam")]
public class BeamSO : ScriptableObject
{
    public float damage;
    public float velocity;
    public float duration;
    public E_Element element;
}
