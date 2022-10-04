using UnityEngine;

namespace Myths
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Myth")]
    public class SO_Myth : ScriptableObject
    {
        public new string name;
        public GameObject prefab;
        public Sprite icon;

        // Stats
        [Range(0,1)] public float health;
        [Range(0, 1)] public float size;
        [Range(0, 1)] public float brawn;
        [Range(0, 1)] public float psyche;
        [Range(1, 5)] public float agility;

        // Abilities
        public SO_Ability[] abilities;
    }
}