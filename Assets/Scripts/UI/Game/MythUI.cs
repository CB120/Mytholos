using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elements
{
    public class MythUI : MonoBehaviour
    {
        [SerializeField] private List<BuffInfo> interfaceList = new();
        public Dictionary<Element, BuffInfo> effectUIData = new();
        public Canvas canvas;
        public HorizontalLayoutGroup parent;
        
        //private GameObject gameCamera;
        private void Awake()
        {
            foreach (BuffInfo info in interfaceList)
                effectUIData.Add(info.element, info);

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
    public struct BuffInfo
    {
        public Element element;
        public BuffUI positiveBuff;
        public BuffUI negativeBuff;
    }
}