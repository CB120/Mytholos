using System.Collections.Generic;
using UnityEngine;
using Elements;
[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Element", order = 1)]
public class SO_Element : ScriptableObject
{
    public Element element;
    public Color color;
    
    // TODO: Move element icon here too

    public List<SO_Element> strongAgainst = new();

    // TODO: Cyclic dependency. Do we need this?
    public bool hasDebris;

    public GameObject abilityDebrisInteractorsPrefab;
    public ParticleSystem buffParticle;
    public ParticleSystem debuffParticle;
}

namespace Elements
{
    public enum Element
    {
        Wood,
        Fire,
        Electric,
        Water,
        Ice,
        Wind,
        Metal,
        Earth
    }
}