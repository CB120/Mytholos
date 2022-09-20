using UnityEngine;

namespace Myths
{
    public class Behaviour : MonoBehaviour
    {
        [SerializeField] protected Myth myth;

        private void Awake()
        {
            enabled = false;
        }
    }
}