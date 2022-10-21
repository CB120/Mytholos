using System.Collections.Generic;
using Myths;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class ElectrifiedDamageDebrisInteractor : DebrisInteractor
    {
        [SerializeField] private Myth myth;
        [SerializeField] private SO_Element electricElement;
        [SerializeField] private bool overrideIfMythIsElectric;
        [SerializeField] private float damagePerSecond;

        private readonly HashSet<Debris> debrisInContact = new();
        private readonly HashSet<Debris> electrifiedDebrisInContact = new();

        private bool IsElectrified => electrifiedDebrisInContact.Count > 0;

        private void Awake()
        {
            if (overrideIfMythIsElectric && myth.element == electricElement)
            {
                enabled = false;
            }
        }

        internal override void OnDebrisEnter(Debris debris)
        {
            AddDebrisContact(debris);
        }

        internal override void OnDebrisExit(Debris debris)
        {
            RemoveDebrisContact(debris);
        }

        private void AddDebrisContact(Debris debris)
        {
            debrisInContact.Add(debris);
            
            debris.isElectrifiedChanged.AddListener(OnIsElectrifiedChanged);
            
            OnIsElectrifiedChanged(debris);
        }

        private void RemoveDebrisContact(Debris debris)
        {
            debrisInContact.Remove(debris);

            electrifiedDebrisInContact.Remove(debris);
            
            debris.isElectrifiedChanged.RemoveListener(OnIsElectrifiedChanged);
        }

        private void OnIsElectrifiedChanged(Debris debris)
        {
            if (debris.IsElectrified)
                electrifiedDebrisInContact.Add(debris);
            else
                electrifiedDebrisInContact.Remove(debris);
        }

        private void Update()
        {
            if (IsElectrified)
                myth.Health.Value -= damagePerSecond * Time.deltaTime;
        }
    }
}