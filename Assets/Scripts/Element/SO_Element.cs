using System.Collections.Generic;
using UnityEngine;
using Elements;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Element", order = 1)]
public class SO_Element : ScriptableObject
{
    public Color color;
    public Sprite icon;
    public Element element;
    public float buffLength;
    public ParticleSystem debuffParticle;
    public ParticleSystem buffParticle;
    public bool setParticleColor;
    // TODO: Move element icon here too

    public List<SO_Element> strongAgainst = new();

    // TODO: Cyclic dependency. Do we need this?
    public bool hasDebris;
    public GameObject abilityDebrisInteractorsPrefab;

    [Header("Description")]
    [TextArea]
    public string buffDescription;
    [TextArea]
    public string debuffDescription;
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