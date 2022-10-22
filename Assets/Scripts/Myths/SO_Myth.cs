using UnityEngine;

namespace Myths
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Myth")]
    public class SO_Myth : ScriptableObject
    {
        public new string name;
        public GameObject prefab;
        public Sprite icon;
        public Sprite iconOff;
        public Sprite iconDead;

        // Stats
        [Range(0,1)] public float health;
        [Range(0, 1)] public float size;
        //[Range(0, 1)] public float brawn;
        //[Range(0, 1)] public float psyche;
        [Range(0, 1)] public float attack;
        [Range(0, 1)] public float agility;

        // Abilities
        public SO_Ability[] abilities;
    }
}