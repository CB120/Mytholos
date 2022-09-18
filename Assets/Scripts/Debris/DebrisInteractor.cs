using UnityEngine;

namespace Debris
{
    // TODO: Ideally when creating a DebrisInteractor, a DebrisInteractorManager should automatically be created, and the DebrisInteractor assigned
    public abstract class DebrisInteractor : MonoBehaviour
    {
        internal virtual void OnDebrisEnter(Debris debris) { }
        
        internal virtual void OnDebrisExit(Debris debris) { }
        
        internal virtual void OnDebrisStay(Debris debris) { }
    }
}