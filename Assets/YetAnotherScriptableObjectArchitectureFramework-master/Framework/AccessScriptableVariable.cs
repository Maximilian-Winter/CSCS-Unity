using OdinSerializer;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{
    public class AccessScriptableVariable <T, V> : MonoBehaviour  where T : Variable<V>
    {
        public T Scriptable;
        public bool InstantiateLocalVariableOnEnable = false;
        public bool CopyVariableValueToLocalVariableOnEnable = false;

        public bool IsInitialized = false;
        
        protected virtual void OnEnable()
        {
            if (InstantiateLocalVariableOnEnable && IsInitialized == false)
            {
                if ( CopyVariableValueToLocalVariableOnEnable )
                {
                    V oldDefaultVal = Scriptable.DefaultValue;
                    V oldRuntimeVal = Scriptable.RuntimeValue();
                    Scriptable = SerializedScriptableObject.CreateInstance<T>();
                    Scriptable.DefaultValue = oldDefaultVal;
                    Scriptable.SetRuntimeValue(oldRuntimeVal);
                }
                else
                {
                    Scriptable = SerializedScriptableObject.CreateInstance<T>();
                }
            }
            IsInitialized = true;
        }


        public void Change(V value)
        {
            if (IsInitialized)
            {
                Scriptable.SetRuntimeValue( value);
            }
        }
    
        public void SetToDefault()
        {
            if (IsInitialized)
            {
                Scriptable.SetRuntimeValue( default);
            }
        }

        public V GetRuntimeValue()
        {
            if (IsInitialized)
            {
                return Scriptable.RuntimeValue();
            }
            else
            {
                return default;
            }
        }
    }
}