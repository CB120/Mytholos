using Commands;
using UnityEngine;

namespace Myths
{
    public class Behaviour : MonoBehaviour
    {
        // TODO: Do we still need this?
        [SerializeField] protected Myth myth;
        [SerializeField] protected MythCommandHandler mythCommandHandler;
        
        public void Awake()
        {
            enabled = false;
        }
    }
}