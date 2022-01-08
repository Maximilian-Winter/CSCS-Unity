using ScriptableObjects.ScriptableArchitecture.Framework;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Settter
{
    public class SetScriptableTransformVariable : AccessScriptableVariable<Variable<Transform>, Transform>
    {
        public Transform TransformToSet;
        protected new void OnEnable()
        {
           base.OnEnable();
           Scriptable.SetRuntimeValue( TransformToSet );
        }
    }
}