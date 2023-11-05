using System.Collections.Generic;
using UnityEngine;
using Elements;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/Element", order = 1)]
public class SO_Element : ScriptableObject
{
    [Header("Appearance")]
    public Color color;
    public Material customMaterial;
    public Sprite icon;
    public Texture2D debrisTexture;
    public Material electrifiedMaterial;

    [Header("Properties")]
    public Element element;
    public float chanceToDebuff;
    public float buffLength;
    public ParticleSystem debuffParticle;
    public ParticleSystem buffParticle;
    public bool setParticleColor;

    // TODO: Convert to HashSet
    public List<SO_Element> strongAgainst = new();
    public List<SO_Element> weakAgainst = new();

    // TODO: Cyclic dependency. Do we need this?
    public bool hasDebris;
    public GameObject abilityDebrisInteractorsPrefab;

    [Header("Description")]
    [TextArea]
    public string buffDescription;
    [TextArea]
    public string debuffDescription;

    //Are these necessary?
    public Color beamStartColor;
    public Color beamEndColor;

    public Color boomerangStartColor;
    public Material boomerangMaterial;

    public Color flurryStartColor;
    public Color flurryEndColor;

    public Color shotStartColor;
    public Color shotEndColor;
}

// TODO: Why do you keep coming back?
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