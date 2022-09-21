using UnityEngine;

namespace Myths
{
    public class Behaviour : MonoBehaviour
    {
        [SerializeField] protected Myth myth;
        public void Awake()
        {
            enabled = false;
        }
    }
}