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

        public void Add(T t)
        {
            if (items.Contains(t)) return;
            
            items.Add(t);
            
            itemAdded.Invoke(t);
        }
        
        public void Remove(T t)
        {
            if (items.Contains(t)) return;
            
            items.Remove(t);
            
            itemRemoved.Invoke(t);
        }
    }
}