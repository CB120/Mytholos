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
        [SerializeField] private GameObject Parent;
        private GameObject gameCamera;
        private void Awake()
        {
            foreach (BuffUI buffUI in interfaceList)
                effectUIData.Add(buffUI.element, buffUI);

            canvas.worldCamera = Camera.main;
            gameCamera = GameObject.FindGameObjectWithTag("GameCamera");
        }

        private void Update()
        {
            //Parent.transform.LookAt(gameCamera.transform);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    [System.Serializable]
    public struct BuffUI
    {
        public Element element;
        public GameObject obj;
        public Animator animator;
    }
}