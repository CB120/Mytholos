using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elements
{
    public class MythUI : MonoBehaviour
    {
        [SerializeField] private List<BuffUI> interfaceList = new();
        public Dictionary<Element, BuffUI> effectUIData = new();
        public Canvas canvas;
        public HorizontalLayoutGroup parent;
        //private GameObject gameCamera;
        private void Awake()
        {
            foreach (BuffUI buffUI in interfaceList)
                effectUIData.Add(buffUI.element, buffUI);

            canvas.worldCamera = Camera.main;
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


    }

    [System.Serializable]
    public struct BuffUI
    {
        public Element element;
        public Animator positiveBuff;
        public Animator negativeBuff;
    }
}