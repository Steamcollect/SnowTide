using System;
using UnityEngine;

namespace ScriptablesObjects
{
    public class WrapperVariable<T> : ScriptableObject
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke();
            }
        }

        public event Action OnChanged;
    }
}
