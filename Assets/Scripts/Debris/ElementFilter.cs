using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Debris
{
    public class ElementFilter : MonoBehaviour
    {
        private enum FilterType
        {
            Is,
            IsNot,
            IsStrongAgainstAny,
            IsWeakAgainstAny,
            IsOrIsStrongAgainstAny,
            IsOrIsWeakAgainstAny
        }
        [Tooltip("The value returned by the filter when the filter list contains the element.")]
        [SerializeField] private FilterType filterType;
        [SerializeField] private List<SO_Element> filterElements;

        public bool PassesFilter(SO_Element element1, SO_Element element2)
        {
            switch (filterType)
            {
                case FilterType.Is:
                    return element1 == element2;
                case FilterType.IsNot:
                    return element1 != element2;
                case FilterType.IsStrongAgainstAny:
                    return element1.strongAgainst.Contains(element2);
                case FilterType.IsWeakAgainstAny:
                    return element2.strongAgainst.Contains(element1);
                case FilterType.IsOrIsStrongAgainstAny:
                    return element1 == element2 ||
                           element1.strongAgainst.Contains(element2);
                case FilterType.IsOrIsWeakAgainstAny:
                    return element1 == element2 ||
                           element2.strongAgainst.Contains(element1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool PassesFilter(SO_Element element)
        {
            switch (filterType)
            {
                case FilterType.Is:
                    return filterElements.Contains(element);
                case FilterType.IsNot:
                    return !filterElements.Contains(element);
                case FilterType.IsStrongAgainstAny:
                    return filterElements.Any(e => element.strongAgainst.Contains(e));
                case FilterType.IsWeakAgainstAny:
                    return filterElements.Any(e => e.strongAgainst.Contains(element));
                case FilterType.IsOrIsStrongAgainstAny:
                    return filterElements.Contains(element) || 
                        filterElements.Any(e => element.strongAgainst.Contains(e));
                case FilterType.IsOrIsWeakAgainstAny:
                    return filterElements.Contains(element) || 
                        filterElements.Any(e => e.strongAgainst.Contains(element));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}