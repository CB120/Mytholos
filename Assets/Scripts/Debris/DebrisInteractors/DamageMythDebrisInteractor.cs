using System.Collections.Generic;
using Myths;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class DamageMythDebrisInteractor : DebrisInteractor
    {
        [SerializeField] private Myth myth;
        [SerializeField] private float damagePerSecond;
        [Tooltip("The value returned by the filter when the filter list contains the element.")]
        [SerializeField] private bool filterType;
        [SerializeField] private List<SO_Element> filterElements;

        // TODO: Add a check for if the myth is of the same element as the debris
        internal override void OnDebrisStay(Debris debris)
        {
            if (filterType == filterElements.Contains(debris.CurrentElement)) 
                myth.Health.Value -= damagePerSecond * Time.deltaTime;
        }
    }
}