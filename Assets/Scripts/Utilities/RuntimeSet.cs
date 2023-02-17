using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        // TODO: Why is the underlying type of our set a list? Are HashSets hard to serialise?
        public readonly List<T> items = new();
        
        [NonSerialized] public UnityEvent<T> itemAdded = new();
        [NonSerialized] public UnityEvent<T> itemRemoved = new();

        [Header("Debug Only")]
        [SerializeField] private int count;

        public void Add(T t)
        {
            if (items.Contains(t)) return;
            
            items.Add(t);

            count = items.Count;
            
            itemAdded.Invoke(t);
        }
        
        public void Remove(T t)
        {
            if (!items.Contains(t)) return;

            items.Remove(t);
            
            count = items.Count;
            
            itemRemoved.Invoke(t);
        }

        public void ListenToAll(Func<T, UnityEvent<T>> unityEventAccessor, UnityAction<T> listener)
        {
            itemAdded.AddListener(t => unityEventAccessor(t).AddListener(listener));
            itemRemoved.AddListener(t => unityEventAccessor(t).RemoveListener(listener));
            
            items.ForEach(t => unityEventAccessor(t).AddListener(listener));
        }
        
        public void UnlistenToAll(Func<T, UnityEvent<T>> unityEventAccessor, UnityAction<T> listener)
        {
            itemAdded.RemoveListener(t => unityEventAccessor(t).AddListener(listener));
            itemRemoved.RemoveListener(t => unityEventAccessor(t).RemoveListener(listener));
            
            items.ForEach(t => unityEventAccessor(t).RemoveListener(listener));
        }
    }
}