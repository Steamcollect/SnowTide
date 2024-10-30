using System;
using UnityEngine;

namespace BT.ScriptablesObject
{
    public class WrapperActionCallback<T> : ScriptableObject
    {
        public event Func<T> action;
        public T Call()
        {
            return action != null ? action.Invoke() : default;
        }
    }
    
    public class WrapperActionCallback<T,T1> : ScriptableObject
    {
        public event Func<T1,T> action;
        public T Call(T1 t1)
        {
            return action != null ? action.Invoke(t1) : default;
        }
    }
    public class WrapperActionCallback<T,T1,T2> : ScriptableObject
    {
        public event Func<T1,T2,T> action;
        public T Call(T1 t1, T2 t2)
        {
            return action != null ? action.Invoke(t1,t2) : default;
        }
    }
}