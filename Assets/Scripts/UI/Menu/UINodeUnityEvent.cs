using UnityEngine;
using UnityEngine.Events;

namespace UI.Menu
{
    public class UINodeUnityEvent : UIMenuNode
    {
        [SerializeField] private UnityEvent unityEvent;

        public override void OnAction(Action action, int playerNumber)
        {
            base.OnAction(action, playerNumber);

            UISFXManager.PlaySound("Confirm");
            
            unityEvent.Invoke();
        }
    }
}