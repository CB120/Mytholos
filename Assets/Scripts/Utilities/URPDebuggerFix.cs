using UnityEngine;
using UnityEngine.Rendering;

namespace DefaultNamespace {

public class URPDebuggerFix : MonoBehaviour
{
    private void Awake()
    {
        DebugManager.instance.enableRuntimeUI = false;
        
    }
}
}