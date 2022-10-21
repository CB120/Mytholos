using UnityEngine;

namespace Debris
{
    [CreateAssetMenu]
    // TODO: Not sure if this is the best way to get access to the DebrisController
    public class DebrisControllerService : ScriptableObject
    {
        public DebrisController DebrisController { get; set; }
    }
}