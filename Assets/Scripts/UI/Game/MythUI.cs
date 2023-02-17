using System.Collections;
using System.Collections.Generic;
using EffectSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Elements
{
    public class MythUI : MonoBehaviour
    {
        [SerializeField] private List<BuffInfo> interfaceList = new();
        public Dictionary<Element, BuffInfo> effectUIData = new();
        //public Canvas canvas;
        public HorizontalLayoutGroup parent;
        Effects effects;
        
        //private GameObject gameCamera;
        private void Awake()
        {
            foreach (BuffInfo info in interfaceList)
                effectUIData.Add(info.element, info);

            //canvas.worldCamera = Camera.main;
            RefreshLayout();
            //gameCamera = GameObject.FindGameObjectWithTag("GameCamera");
        }


        private void Update()
        {
            //Parent.transform.LookAt(gameCamera.transform);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void RefreshLayout()
        {
            parent.enabled = false;
            parent.enabled = true;
        }

        public void ActivateBuff(SO_Element element, bool isDebuff)
        {
            RefreshLayout();

            if (isDebuff)
            {
                effectUIData[element.element].negativeBuff.gameObject.SetActive(true);
                effectUIData[element.element].negativeBuff.isEnabled = true;
            }
            else
            {
                effectUIData[element.element].positiveBuff.gameObject.SetActive(true);
                effectUIData[element.element].positiveBuff.isEnabled = true;
            }
        }

        public void DeactivateBuff(SO_Element element, bool isDebuff, bool hasDebuff, bool hasBuff)
        {
            RefreshLayout();

            if (isDebuff && hasDebuff)
            {
                effectUIData[element.element].negativeBuff.isEnabled = false;
            }
            else if (!isDebuff && hasBuff)
            {
                effectUIData[element.element].positiveBuff.isEnabled = false;
            }
        }

        public void UpdateListeners(Effects newEffects)
        {
            if (effects != null)
            {
                effects.ActivateBuffEvent.RemoveListener(ActivateBuff);
                effects.DeactivateBuffEvent.RemoveListener(DeactivateBuff);

                // Look though all active buff effects on old effects script, and remove them from this canvas
                foreach (SO_Element element in effects.appliedBuffs)
                    DeactivateBuff(element, false, false, true);

                foreach (SO_Element element in effects.appliedDebuffs)
                    DeactivateBuff(element, true, true, false);
            }

            effects = newEffects;
            effects.ActivateBuffEvent.AddListener(ActivateBuff);
            effects.DeactivateBuffEvent.AddListener(DeactivateBuff);
        }
    }

    [System.Serializable]
    public struct BuffInfo
    {
        public Element element;
        public BuffUI positiveBuff;
        public BuffUI negativeBuff;
    }
}