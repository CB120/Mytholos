using UnityEngine;
using Elements;

namespace Myths
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Myth")]
    public class SO_Myth : ScriptableObject
    {
        public new string name;
        public GameObject prefab;
        public Sprite icon;
        public Element element;
    }
}